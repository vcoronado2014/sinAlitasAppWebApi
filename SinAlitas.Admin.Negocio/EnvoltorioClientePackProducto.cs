using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinAlitas.Admin.Negocio
{
    public class EnvoltorioClientePackProducto
    {
        public static Entidad.EnvoltorioClientePackProducto Obtener(int pcoId)
        {
            Entidad.EnvoltorioClientePackProducto env = new Entidad.EnvoltorioClientePackProducto();
            env.ProductoCodigo = Negocio.ProductoCodigo.ObtenerProductosCodigoPorId(pcoId)[0];
            if (env.ProductoCodigo != null && env.ProductoCodigo.Id > 0)
            {
                env.Cliente = Negocio.Cliente.ObtenerClientePorId(env.ProductoCodigo.ClieId);
                env.Comuna = Negocio.Territorio.ObtenerComunanPorId(env.Cliente.ComId);
                env.FichaPack = Negocio.FichaPack.ObtenerFichasPack(env.ProductoCodigo.Id);
                env.Condiciones = Negocio.AceptaCondiciones.ObtenerAceptaCondicionesPcoId(env.ProductoCodigo.Id);
                env.CuposTomados = Negocio.Cupo.ListarCuposProductoCodigo(env.ProductoCodigo.Id);
            }

            return env;
        }
    }
}
