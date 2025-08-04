using DL;
using Microsoft.EntityFrameworkCore;
using ML;

namespace BL
{
    public class Usuario
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.GromeroProgramacionNcapasContext context = new DL.GromeroProgramacionNcapasContext())
                {
                    var listaUsuario = (from usuarioDB in context.Usuarios
                                        join rolDB in context.Rols
                                        on usuarioDB.IdRol equals rolDB.IdRol
                                        select new
                                        {
                                            usuarioDB.IdUsuario,
                                            usuarioDB.UserName,
                                            usuarioDB.Nombre,
                                            usuarioDB.ApellidoPaterno,
                                            usuarioDB.ApellidoMaterno,
                                            usuarioDB.Email,
                                            usuarioDB.Password,
                                            usuarioDB.Sexo,
                                            usuarioDB.Telefono,
                                            usuarioDB.Celular,
                                            usuarioDB.FechaNacimiento,
                                            usuarioDB.Curp,
                                            rolDB.IdRol,
                                            rolDB.NombreRol
                                        }).ToList();

                    result.Objects = new List<object>();

                    foreach (var usuarioObj in listaUsuario)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = usuarioObj.IdUsuario;
                        usuario.UserName = usuarioObj.UserName;
                        usuario.Nombre = usuarioObj.Nombre;
                        usuario.ApellidoPaterno = usuarioObj.ApellidoPaterno;
                        usuario.ApellidoMaterno = usuarioObj.ApellidoMaterno;
                        usuario.Email = usuarioObj.Email;
                        usuario.Password = usuarioObj.Password;
                        usuario.Sexo = usuarioObj.Sexo;
                        usuario.Telefono = usuarioObj.Telefono;
                        usuario.Celular = usuarioObj.Celular;
                        //usuario.FechaNacimiento = usuarioObj.FechaNacimiento;
                        usuario.CURP = usuarioObj.Curp;

                        usuario.Rol = new ML.Rol
                        {
                            IdRol = usuarioObj.IdRol,
                            NombreRol = usuarioObj.NombreRol
                        };

                        result.Objects.Add(usuario);
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

        public static ML.Result GetByIdLINQ(int id)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.GromeroProgramacionNcapasContext context = new DL.GromeroProgramacionNcapasContext())
                {
                    var query = (from u in context.Usuarios
                                 where u.IdUsuario == id
                                 select u).FirstOrDefault();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.UserName = query.UserName;
                        usuario.Nombre = query.Nombre;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.Email = query.Email;
                        usuario.Password = query.Password;
                        usuario.Sexo = query.Sexo;
                        usuario.Telefono = query.Telefono;
                        usuario.Celular = query.Celular;
                        //usuario.CURP = query.CURP;
                        //usuario.FechaNacimiento = query.FechaNacimiento;
                        usuario.Rol = new ML.Rol
                        {
                            IdRol = query.IdRol.Value
                        };

                        result.Object = usuario;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
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


        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.GromeroProgramacionNcapasContext context = new DL.GromeroProgramacionNcapasContext())
                {
                    DL.Usuario usuarioDL = new DL.Usuario();

                    usuarioDL.UserName = usuario.UserName;
                    usuarioDL.Password = usuario.Password;
                    usuarioDL.Nombre = usuario.Nombre;
                    usuarioDL.ApellidoPaterno = usuario.ApellidoPaterno;
                    usuarioDL.ApellidoMaterno = usuario.ApellidoMaterno;
                    usuarioDL.Email = usuario.Email;
                    //usuarioDL.FechaNacimiento = usuario.FechaNacimiento;
                    usuarioDL.Sexo = usuario.Sexo;
                    usuarioDL.Telefono = usuario.Telefono;
                    usuarioDL.Celular = usuario.Celular;
                    usuarioDL.Curp = usuario.CURP;
                    usuarioDL.IdRol = usuario.Rol.IdRol;

                    context.Usuarios.Add(usuarioDL);
                    int filasAfectadas = context.SaveChanges();

                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }

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

        public static ML.Result DeleteLQ(int IdUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (GromeroProgramacionNcapasContext context = new GromeroProgramacionNcapasContext())
                {
                    var query = (from a in context.Usuarios
                                 where a.IdUsuario == IdUsuario
                                 select a).FirstOrDefault();
                        
                    context.Usuarios.Remove(query);

                    int filasAfectadas = context.SaveChanges();

                    result.Correct = filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMesage = ex.Message;
            }
            return result;
        }

        public static ML.Result Delete(int Id)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.GromeroProgramacionNcapasContext context = new DL.GromeroProgramacionNcapasContext())
                {

                    var usuario = context.Usuarios.Find(Id);

                    if (usuario != null)
                    {
                        context.Usuarios.Remove(usuario);
                        context.SaveChanges();

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
            }

            return result;
        }

        public static ML.Result UpdateLQ(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.GromeroProgramacionNcapasContext context = new DL.GromeroProgramacionNcapasContext())
                {
                    var query = (from u in context.Usuarios
                                 where u.IdUsuario == usuario.IdUsuario
                                 select u).FirstOrDefault();

                    if (query != null)
                    {
                        query.UserName = usuario.UserName;
                        query.Nombre = usuario.Nombre;
                        query.ApellidoPaterno = usuario.ApellidoPaterno;
                        query.ApellidoMaterno = usuario.ApellidoMaterno;
                        query.Email = usuario.Email;
                        query.Password = usuario.Password;
                        query.Sexo = usuario.Sexo;
                        query.Telefono = usuario.Telefono;
                        query.Celular = usuario.Celular;                      
                        query.IdRol = usuario.Rol.IdRol;
                        context.SaveChanges();
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
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


    }
}
