using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Rol
    {

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.GromeroProgramacionNcapasContext context = new DL.GromeroProgramacionNcapasContext())
                {
                    var query = context.Rols.ToList();
                    result.Objects = new List<object>();

                    foreach (var item in query)
                    {
                        ML.Rol rol = new ML.Rol();
                        rol.IdRol = item.IdRol;
                        rol.NombreRol = item.NombreRol;

                        result.Objects.Add(rol);
                    }

                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMesage = ex.Message;
                result.exception = ex;
            }

            return result;
        }


    }
}
