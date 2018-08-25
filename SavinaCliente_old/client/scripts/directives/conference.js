(function (ng) {
  'use strict';

  function logError() {
    console.error.apply(console, arguments);
  }

  ng.module('App').directive('conference', [
    '$rootScope', '$routeParams', '$io',

    function ($rootScope, $routeParams, $io) {

      return {
        templateUrl: '/assets/templates/conference.html',
        restrict: 'E',

        scope: {
          active: '=',
          busy: '='
        },

        link: function ($scope, $element, $attrs) {
          var socket, peerconn, localStream, connected;
          var namespace = '/conference';

          var configuration = {
            iceServers: [{
              url: 'stun:ec2-54-145-137-159.compute-1.amazonaws.com'
            }, {
              url: 'turn:ec2-54-145-137-159.compute-1.amazonaws.com',
              credential: 'savina',
              username: 'savina'
            }, {
              url: 'stun:stun.l.google.com:19302'
            }, {
              url: 'stun:stun1.l.google.com:19302'
            }, {
              url: 'stun:stun2.l.google.com:19302'
            }, {
              url: 'stun:stun3.l.google.com:19302'
            }, {
              url: 'stun:stun4.l.google.com:19302'
            }]
          };

          var options = {
            optional: [{
              DtlsSrtpKeyAgreement: true
            }, {
              RtpDataChannels: false
            }]
          };

          var constraints = {
            mandatory: {
              OfferToReceiveAudio: true,
              OfferToReceiveVideo: true
            }
          };

          var mediaOptions = {
            video: true,
            audio: true
          };

          var $localVideo = $element.find($attrs.localVideo);
          var $remoteVideo = $element.find($attrs.remoteVideo);

          $scope.expanded = false;

          $remoteVideo.on('click', function () {
            $scope.expanded = !$scope.expanded;
            $scope.$apply();
          });

          /**
           * socketing listeners.
           */

          function onconnect() {
            $scope.$apply(function () {
              connected = true;

              requestUserMedia();
            });
          }

          function ondisconnect() {
            $scope.$apply(function () {
              $scope.active = false;
              $scope.busy = false;

              connected = false;
              socket = null;
            });
          }

          function oncreated() {
            console.log("You have created this room");
          }

          function onjoined() {
            console.log("You have joined this room");
            createOffer();
          }

          function onoffer(offer) {
            console.log("A peer has sent you an offer");
            createAnswer(offer);
          }

          function onanswer(answer) {
            console.log("A peer has answered your offer");

            peerconn.setRemoteDescription(new RTCSessionDescription(answer), function () {
              console.log("setRemoteDescription OK!");
            }, function (err) {
              console.error(err);
            });
          }

          function onicecandidate(candidate) {
            console.log("A peer has sent you an ICE candidate");

            peerconn.addIceCandidate(new RTCIceCandidate(candidate), function () {
              console.log("addIceCandidate OK!");
            }, logError);
          }

          function onpeerleft() {
            console.log("Peer left");

            if (peerconn) {
              console.log("Closing peer connection");
              peerconn.close();
              peerconn = null;
            }

            $remoteVideo.empty();
          }

          /**
           * Events.
           */

          function openSignaling() {
            socket = $io.connect(namespace, {
              multiplex: false
            });

            socket.on('connect', onconnect);
            socket.on('created', oncreated);
            socket.on('joined', onjoined);
            socket.on('offer', onoffer);
            socket.on('answer', onanswer);
            socket.on('ice candidate', onicecandidate);
            socket.on('peer left', onpeerleft);
            socket.on('disconnect', ondisconnect);
          }

          /** Join the room */
          function joinRoom() {
            socket.emit('join', $attrs.room);
          }

          /** Append a video element */
          function addVideo(local, stream) {
            var $video = ng.element('<video></video>');

            $video.attr('autoplay', 'autoplay');

            if (local) {
              $video.attr('muted', 'muted');
              $localVideo.append($video);
            } else {
              $remoteVideo.append($video);
            }

            $video[0].src = URL.createObjectURL(stream);

            $scope.busy = false;
          }

          function requestUserMedia() {
            navigator.getUserMedia(mediaOptions, function (stream) {
              localStream = stream;

              addVideo(true, localStream);

              joinRoom();
            }, logError);
          }

          /** Create the RTCPeerConnection */
          function createPeerConnection() {
            var pc = new RTCPeerConnection(configuration, options);

            pc.onicecandidate = function (event) {
              if (event.candidate) {
                console.log("Got ICE candidate");
                socket.emit('ice candidate', event.candidate);
              }
            };

            pc.onaddstream = function (event) {
              console.log("Adding remote stream");
              addVideo(false, event.stream);
            };

            pc.oniceconnectionstatechange = function (event) {
              console.log("oniceconnectionstatechange", event);
            };

            return pc;
          }

          /** Create a connection offer */
          function createOffer() {
            peerconn = createPeerConnection();

            peerconn.addStream(localStream);

            peerconn.createOffer(function (offer) {
              peerconn.setLocalDescription(offer, function () {
                socket.emit('offer', offer);
              }, logError);
            }, logError, constraints);
          }

          /** Create an answer for an offer */
          function createAnswer(offer) {
            peerconn = createPeerConnection();

            peerconn.setRemoteDescription(new RTCSessionDescription(offer), function () {
              peerconn.addStream(localStream);

              peerconn.createAnswer(function (answer) {
                peerconn.setLocalDescription(answer, function () {
                  socket.emit('answer', answer);
                }, logError);
              }, logError, constraints);
            }, logError);
          }

          /** Initialize the connection */
          function start() {
            $scope.active = true;
            $scope.busy = false;

            if (connected) {
              requestUserMedia();
            } else {
              openSignaling(); /* Open the signaling channel */
            }
          }

          function stopTrack(track) {
            track.stop();
          }

          function stop() {
            if (connected) {
              $localVideo.empty();
              $remoteVideo.empty();

              socket.emit('leave');
            }

            if (localStream && localStream.active) {
              ng.forEach(localStream.getVideoTracks(), stopTrack);
              ng.forEach(localStream.getAudioTracks(), stopTrack);

              localStream = null;
            }
          }

          $scope.active = false;
          $scope.busy = false;

          $scope.start = start;
          $scope.stop = stop;

          $scope.$on('$destroy', function () {
            if ($scope.active) {
              stop();
            }
          });

          $scope.$watch('active', function (active) {
            (active && start || stop)();
          });

        }
      };

    }

  ]);

}(angular));
