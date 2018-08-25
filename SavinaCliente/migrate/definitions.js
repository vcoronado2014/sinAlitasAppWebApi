'use strict';

module.exports = [

  /* Roles */
  {
    sourceTable: 'static_roles',
    destTable:   'roles',

    sourceFields: ['id', 'name', 'slug'],
    destFields:   ['id', 'name', 'slug']
  },

  /* Specialties */
  {
    sourceTable: 'static_specialties',
    destTable:   'specialties',

    sourceFields: ['id', 'name', 'slug'],
    destFields:   ['id', 'name', 'slug']
  },

  /* Genders */
  {
    sourceTable: 'static_genders',
    destTable:   'genders',

    sourceFields: ['id', 'name', 'slug', 'value'],
    destFields:   ['id', 'name', 'slug', 'value']
  },

  /* Checkup Modes */
  {
    sourceTable: 'static_checkups',
    destTable:   'checkupModes',

    sourceFields: ['id', 'name', 'slug'],
    destFields:   ['id', 'name', 'slug']
  },

  /* Motives */
  {
    sourceTable: 'static_motives',
    destTable:   'motives',

    sourceFields: ['id', 'name', 'slug'],
    destFields:   ['id', 'name', 'slug']
  },

  /* Priorities */
  {
    sourceTable: 'static_priorities',
    destTable:   'priorities',

    sourceFields: ['id', 'name', 'slug'],
    destFields:   ['id', 'name', 'slug']
  },

  /* Request Types */
  {
    sourceTable: 'static_request_types',
    destTable:   'requestTypes',

    sourceFields: ['id', 'name', 'slug'],
    destFields:   ['id', 'name', 'slug']
  },

  /* Users */
  {
    sourceTable: 'users',
    destTable:   'users',

    sourceFields: ['id', 'name', 'email', 'password', 'deletedAt', 'createdAt', 'updatedAt', 'role',   'creator',   'specialty'],
    destFields:   ['id', 'name', 'email', 'password', 'deletedAt', 'createdAt', 'updatedAt', 'roleId', 'creatorId', 'specialtyId']
  },

  /* Preferences */
  {
    sourceTable: 'preferences',
    destTable:   'preferences',

    sourceFields: ['id', 'email_request_attached', 'email_request_answered', 'email_request_returned', 'email_request_messaged', 'email_request_closed', 'email_request_taken', 'createdAt', 'updatedAt', 'user'],
    destFields:   ['id', 'emailRequestAttached',   'emailRequestAnswered',   'emailRequestReturned',   'emailRequestMessaged',   'emailRequestClosed',   'emailRequestTaken',   'createdAt', 'updatedAt', 'userId']
  },

  /* Tokens */
  {
    sourceTable: 'tokens',
    destTable:   'tokens',

    sourceFields: ['id', 'secret', 'createdAt', 'updatedAt', 'user'],
    destFields:   ['id', 'secret', 'createdAt', 'updatedAt', 'userId']
  },

  /* Workplaces */
  {
    sourceTable: 'workplaces',
    destTable:   'workplaces',

    sourceFields: ['id', 'name', 'private', 'deletedAt', 'createdAt', 'updatedAt', 'creator'],
    destFields:   ['id', 'name', 'private', 'deletedAt', 'createdAt', 'updatedAt', 'creatorId']
  },

  /* User Workplaces */
  {
    sourceTable: 'user_workplaces__workplace_users',
    destTable:   'userWorkplaces',

    sourceFields: ['id', 'user_workplaces', 'workplace_users'],
    destFields:   ['id', 'userId',          'workplaceId']
  },

  /* Agreements */
  {
    sourceTable: 'agreements',
    destTable:   'agreements',

    sourceFields: ['id', 'name', 'quota', 'createdAt', 'updatedAt', 'deletedAt', 'creator',   'specialty'],
    destFields:   ['id', 'name', 'quota', 'createdAt', 'updatedAt', 'deletedAt', 'creatorId', 'specialtyId']
  },

  /* Agreements Workplaces */
  {
    sourceTable: 'agreement_workplaces__workplace_agreements',
    destTable:   'agreementWorkplaces',

    sourceFields: ['id', 'agreement_workplaces', 'workplace_agreements'],
    destFields:   ['id', 'agreementId',          'workplaceId']
  },

  /* Patients */
  {
    sourceTable: 'patients',
    destTable:   'patients',

    sourceFields: ['id', 'firstname', 'lastname', 'run', 'birth', 'city', 'createdAt', 'updatedAt', 'gender'],
    destFields:   ['id', 'firstname', 'lastname', 'run', 'birth', 'city', 'createdAt', 'updatedAt', 'genderId']
  },

  /* Requests */
  {
    sourceTable: 'requests',
    destTable:   'requests',

    sourceFields: ['id', 'hypothesis', 'comment', 'createdAt', 'updatedAt', 'closedAt', 'type',          'creator_user',  'creator_workplace',  'specialty',   'patient',   'motive',   'priority',   'specialist_user',  'specialist_workplace'],
    destFields:   ['id', 'hypothesis', 'comment', 'createdAt', 'updatedAt', 'closedAt', 'requestTypeId', 'creatorUserId', 'creatorWorkplaceId', 'specialtyId', 'patientId', 'motiveId', 'priorityId', 'specialistUserId', 'specialistWorkplaceId']
  },

  /* Request Messages */
  {
    sourceTable: 'request_messages',
    destTable:   'requestMessages',

    sourceFields: ['id', 'type', 'body', 'sentAt', 'receivedAt', 'createdAt', 'updatedAt', 'request',   'sender_user', 'sender_workplace'],
    destFields:   ['id', 'type', 'body', 'sentAt', 'receivedAt', 'createdAt', 'updatedAt', 'requestId', 'userId',      'workplaceId']
  },

  /* Request Attachments */
  {
    sourceTable: 'request_attachments',
    destTable:   'requestAttachments',

    sourceFields: ['id', 'name', 'description', 'seenAt', 'createdAt', 'updatedAt', 'request',   'creator_user', 'creator_workplace'],
    destFields:   ['id', 'name', 'description', 'seenAt', 'createdAt', 'updatedAt', 'requestId', 'userId',       'workplaceId']
  },

  /* Request Answers */
  {
    sourceTable: 'request_answers',
    destTable:   'requestAnswers',

    sourceFields: ['id', 'comment', 'seenAt', 'createdAt', 'updatedAt', 'request',   'creator_user', 'creator_workplace'],
    destFields:   ['id', 'comment', 'seenAt', 'createdAt', 'updatedAt', 'requestId', 'userId',       'workplaceId']
  },

  /* Request Answer Diagnoses */
  {
    sourceTable: 'request_answer_diagnoses',
    destTable:   'requestAnswerDiagnoses',

    sourceFields: ['id', 'description', 'evaluation', 'confirmed', 'therapeutic_plan', 'checkup_date', 'checkup_exams_required', 'checkup_same_specialist', 'checkup_comment', 'createdAt', 'updatedAt', 'request_answer',  'checkup_mode'],
    destFields:   ['id', 'description', 'evaluation', 'confirmed', 'therapeuticPlan',  'checkupDate',  'checkupExamsRequired',   'checkupSameSpecialist',   'checkupComment',  'createdAt', 'updatedAt', 'requestAnswerId', 'checkupModeId']
  },

  /* Request Answer Diagnosis Exams */
  {
    sourceTable: 'request_answer_diagnosis_exams',
    destTable:   'requestAnswerDiagnosisExams',

    sourceFields: ['id', 'name', 'same_place', 'createdAt', 'updatedAt', 'request_answer_diagnosis'],
    destFields:   ['id', 'name', 'samePlace',  'createdAt', 'updatedAt', 'requestAnswerDiagnosisId']
  },

  /* Files */
  {
    sourceTable: 'files',
    destTable:   'files',

    sourceFields: ['id', 'mimetype', 'filename', 'filesize', 'path', 'md5', 'createdAt', 'updatedAt'],
    destFields:   ['id', 'mimetype', 'filename', 'filesize', 'path', 'md5', 'createdAt', 'updatedAt']
  },

  /* File Request Attachments */
  {
    sourceTable: 'file_request_attachment__request_attachment_files',
    destTable:   'fileRequestAttachments',

    sourceFields: ['id', 'file_request_attachment', 'request_attachment_files'],
    destFields:   ['id', 'fileId',                  'requestAttachmentId']
  },

  /* ICDs */
  {
    sourceTable: 'icds',
    destTable:   'icds',

    sourceFields: ['id', 'code', 'description'],
    destFields:   ['id', 'code', 'description']
  },
  /* ICDs Requests */
  {
    sourceTable: 'icd_request_icds__request_icds',
    destTable:   'icdRequests',

    sourceFields: ['id', 'icd_request_icds', 'request_icds'],
    destFields:   ['id', 'icdId',            'requestId']
  },

  /* ICDs Request Answer Diagnoses */
  {
    sourceTable: 'icd_request_answer_diagnosis_icds__request_answer_diagnosis_icds',
    destTable:   'icdRequestAnswerDiagnoses',

    sourceFields: ['id', 'icd_request_answer_diagnosis_icds', 'request_answer_diagnosis_icds'],
    destFields:   ['id', 'icdId',                             'requestAnswerDiagnosisId']
  },

  /* Notifications */
  {
    sourceTable: 'notifications',
    destTable:   'notifications',

    sourceFields: ['id', 'foreignKey', 'action', 'model', 'createdAt', 'updatedAt', 'seenAt', 'sender',   'receiver',   'workplace'],
    destFields:   ['id', 'foreignKey', 'action', 'model', 'createdAt', 'updatedAt', 'seenAt', 'senderId', 'receiverId', 'workplaceId']
  }

];
