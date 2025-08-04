using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PL.Controllers
{
    public class UsuarioController1 : Controller
    {

        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            usuario.Nombre = "";
            usuario.ApellidoPaterno = "";
            usuario.ApellidoMaterno = "";
            usuario.Rol.IdRol = 0;

            ML.Result result = BL.Usuario.GetAllLike(usuario);
            ML.Result resultRoles = BL.Rol.GetAll();

            usuario.Rol.Roles = resultRoles.Correct ? resultRoles.Objects : new List<object>();
            usuario.Usuarios = result.Correct ? result.Objects : new List<object>();

            return View(usuario);
        }

        [HttpPost]
        public IActionResult GetAll(ML.Usuario usuario)
        {
            if (usuario.Rol == null)
                usuario.Rol = new ML.Rol();

            ML.Result result = BL.Usuario.GetAllLike(usuario);
            ML.Result resultRoles = BL.Rol.GetAll();

            usuario.Rol.Roles = resultRoles.Correct ? resultRoles.Objects : new List<object>();

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
            }
            else
            {
                ViewBag.Mensaje = result.ErrorMesage;
                usuario.Usuarios = new List<object>();
            }

            return View(usuario);
        }

        public static ML.Result GetAllLike(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.GromeroProgramacionNcapasContext context = new DL.GromeroProgramacionNcapasContext())
                {
                    var query = context.UsuarioGetAllDTOs
                        .FromSqlRaw("EXEC UsuarioLikeCompleto @p0, @p1, @p2, @p3",
                            usuario.Nombre ?? "",
                            usuario.ApellidoPaterno ?? "",
                            usuario.ApellidoMaterno ?? "",
                            usuario.Rol?.IdRol ?? 0)
                        .ToList();

                    if (query != null && query.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var usuarioDB in query)
                        {
                            ML.Usuario user = new ML.Usuario();
                            user.Rol = new ML.Rol();

                            user.IdUsuario = usuarioDB.IdUsuario;
                            user.Nombre = usuarioDB.Nombre;
                            user.ApellidoPaterno = usuarioDB.ApellidoPaterno;
                            user.ApellidoMaterno = usuarioDB.ApellidoMaterno;
                            user.Email = usuarioDB.Email;
                            user.Password = usuarioDB.Password;
                            user.Sexo = usuarioDB.Sexo;
                            user.Telefono = usuarioDB.Telefono;
                            user.Celular = usuarioDB.Celular;
                            user.Rol.IdRol = usuarioDB.IdRol;
                            user.Rol.NombreRol = usuarioDB.NombreRol;

                            result.Objects.Add(user);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMesage = "No se encontraron usuarios.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMesage = ex.Message;
            }

            return result;
        }



        //public IActionResult GetAll()
        //{
        //    ML.Usuario usuario = new ML.Usuario();
        //    usuario.Rol = new ML.Rol();

        //    usuario.Nombre = "";
        //    usuario.ApellidoPaterno = "";
        //    usuario.ApellidoMaterno = "";

        //    ML.Result result = BL.Usuario.GetAllLike(usuario);
        //    ML.Result resultRoles = BL.Rol.GetAll();

        //    usuario.Rol.Roles = resultRoles.Correct ? resultRoles.Objects : new List<object>();
        //    usuario.Usuarios = result.Correct ? result.Objects : new List<object>();


        //    BL.Usuario.GetAllLike(usuario);

        //    return View(usuario);
        //}

        //[HttpPost]
        //public IActionResult GetAll(ML.Usuario usuario, String Nombre, String ApellidoPaterno, String ApellidoMaterno, String Action)
        //{
        //    usuario.Rol = new ML.Rol();

        //    ML.Result result = BL.Usuario.GetAll();
        //    ML.Result resultRoles = BL.Rol.GetAll();

        //    usuario.Rol.Roles = resultRoles.Correct ? resultRoles.Objects : new List<object>();
        //    usuario.Usuarios = result.Correct ? result.Objects : new List<object>();

        //    BL.Usuario.GetAllLike(usuario);


        //    return View(usuario);
        //}

        //[HttpGet]
        //public IActionResult GetAllLike()
        //{
        //    ML.Usuario usuario = new ML.Usuario();
        //    usuario.Rol = new ML.Rol();

        //    ML.Result result = BL.Usuario.GetAll();
        //    ML.Result resultRoles = BL.Rol.GetAll();

        //    usuario.Rol.Roles = resultRoles.Correct ? resultRoles.Objects : new List<object>();
        //    usuario.Usuarios = result.Correct ? result.Objects : new List<object>();

        //    return View(usuario);
        //}

        //[HttpPost]
        //public IActionResult GetAllLike(ML.Usuario usuario)
        //{
        //    ML.Result result = BL.Usuario.GetAllLike(usuario);

        //    usuario.Rol = new ML.Rol();
        //    ML.Result resultRoles = BL.Rol.GetAll();

        //    if (resultRoles.Correct)
        //    {
        //        usuario.Rol.Roles = resultRoles.Objects;
        //    }

        //    if (result.Correct)
        //    {
        //        usuario.Usuarios = result.Objects;
        //        return View("GetAll", usuario);
        //    }
        //    else
        //    {
        //        ViewBag.Mensaje = result.ErrorMesage;
        //        usuario.Usuarios = new List<object>();
        //        return View("GetAll", usuario);
        //    }
        //}

        [HttpGet]
        public IActionResult GetAdd(int? Id)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();

            ML.Result resultRoles = BL.Rol.GetAll();

            if (resultRoles.Correct)
            {
                usuario.Rol.Roles = resultRoles.Objects;
            }

            if (Id > 0) // Update
            {
                ML.Result result = BL.Usuario.GetByIdLINQ(Id.Value);

                if (result.Correct)
                {
                    usuario = (ML.Usuario)result.Object;
                    usuario.Rol.Roles = resultRoles.Objects;


                }
            }

            return View(usuario);
        }


        [HttpPost]
        public IActionResult GetAdd(ML.Usuario usuario)
        {
            ML.Result resultRoles = BL.Rol.GetAll();

                if (usuario.IdUsuario == 0) // Add
                {
                    ML.Result result = BL.Usuario.Add(usuario);     
                }
                else // Update
                {
                    // Actualiza usuario
                    ML.Result resul = BL.Usuario.UpdateLQ(usuario);

                }
            
            return RedirectToAction("GetAll");
        }



        [HttpGet]
            public IActionResult Delete(int IdUsuario)
            {
                // Eliminar las imagenes del usuario

                ML.Result resultuser = BL.Usuario.Delete(IdUsuario);

                if (resultuser.Correct)
                {
                    // Elimino al usuario
                    ML.Result resultUsuario = BL.Usuario.DeleteLQ(IdUsuario);
                }

                return RedirectToAction("GetAll");

            }

            [HttpPost]
            public JsonResult DeleteAjax(int Id)
            {
                var result = BL.Usuario.Delete(Id);
                return Json(new { success = result.Correct, message = result.ErrorMesage });
            }

        }
    }
