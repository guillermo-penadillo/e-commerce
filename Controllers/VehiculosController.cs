using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Pr_Concesionaria.Models;
using System.Runtime.CompilerServices;

namespace Pr_Concesionaria.Controllers
{
    [Authorize]
    public class VehiculosController : Controller
    {
        public readonly IConfiguration configuration;

        public VehiculosController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        //Metodo listar vehiculo
        IEnumerable<Vehiculo> obtenerVehiculo()
        {
            List<Vehiculo> vehiculos = new List<Vehiculo>();
            using (SqlConnection cn = new SqlConnection(configuration["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("select * from tbl_vehiculo", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    vehiculos.Add(new Vehiculo
                    {
                        IdVehiculo = dr.GetInt32(0),
                        IdModelo = dr.GetInt32(1),
                        IdColor = dr.GetInt32(2),
                        IdTransmision = dr.GetInt32(3),
                        Precio = dr.GetDecimal(4),
                        Stock = dr.GetInt32(5),
                    });
                }
                return vehiculos;
            }
        }

        //Metodo listar todos los vehiculos
        IEnumerable<Vehiculo> Get()
        {
            List<Vehiculo> vehiculos = new List<Vehiculo>();
            using (SqlConnection cn = new SqlConnection(configuration["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_listar_vehiculo", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    vehiculos.Add(new Vehiculo
                    {
                        IdVehiculo = dr.GetInt32(0),
                        Marca = dr.GetString(1),
                        Modelo = dr.GetString(2),
                        TipoCombustible = dr.GetString(3),
                        Color = dr.GetString(4),
                        Transmision = dr.GetString(5),
                        Categoria = dr.GetString(6),
                        Precio = dr.GetDecimal(7),
                        Stock = dr.GetInt32(8),
                    });
                }
                return vehiculos;
            }
        }

        //Vista listar vehiculos
        public async Task<IActionResult> ListadoVehiculos(int p)
        {
            int nr = 8;
            int tr = Get().Count();
            int paginas = nr % tr > 0 ? tr / nr + 1 : tr / nr;
            ViewBag.paginas = paginas;
            return View(await Task.Run(() => Get().Skip(p * nr).Take(nr)));
        }

        //select modelo
        IEnumerable<Modelo> Modelos()
        {
            List<Modelo> modelos = new List<Modelo>();
            using (SqlConnection cn = new SqlConnection(configuration["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("select * from tbl_modelo", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    modelos.Add(new Modelo()
                    {
                        IdModelo = dr.GetInt32(0),
                        NombreModelo = dr.GetString(2),
                    });
                }
                return modelos;
            }
        }

        //select color
        IEnumerable<Color> Colors()
        {
            List<Color> colors = new List<Color>();
            using (SqlConnection cn = new SqlConnection(configuration["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("select * from tbl_color", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    colors.Add(new Color()
                    {
                        IdColor = dr.GetInt32(0),
                        NombreColor = dr.GetString(1),
                    });
                }
                return colors;
            }
        }

        //select transmision
        IEnumerable<Transmision> Transmisions()
        {
            List<Transmision> transmisions = new List<Transmision>();
            using (SqlConnection cn = new SqlConnection(configuration["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("select * from tbl_transmision", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    transmisions.Add(new Transmision()
                    {
                        IdTransmision = dr.GetInt32(0),
                        NombreTransmision = dr.GetString(1),
                    });
                }
                return transmisions;
            }
        }

        //Vista insertar vehiculo
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.modelos = new SelectList(await Task.Run(() => Modelos()), "IdModelo", "NombreModelo");
            ViewBag.transmisions = new SelectList(await Task.Run(() => Transmisions()), "IdTransmision", "NombreTransmision");
            ViewBag.colors = new SelectList(await Task.Run(() => Colors()), "IdColor", "NombreColor");
            Vehiculo vehiculo = new Vehiculo();
            return View(vehiculo);
        }

        //Metodo insertar vehiculo
        [HttpPost]
        public async Task<IActionResult> Create(Vehiculo vehiculo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "El modelo no es válido. Por favor, revisa los campos.";

                ViewBag.modelos = new SelectList(await Task.Run(() => Modelos()), "IdModelo", "NombreModelo", vehiculo.IdModelo);
                ViewBag.transmisions = new SelectList(await Task.Run(() => Transmisions()), "IdTransmision", "NombreTransmision", vehiculo.IdTransmision);
                ViewBag.colors = new SelectList(await Task.Run(() => Colors()), "IdColor", "NombreColor", vehiculo.IdColor);
                return View(vehiculo);
            }
            try
            {
                string mensaje = "";
                using (SqlConnection cn = new SqlConnection(configuration["ConnectionStrings:cn"]))
                {
                    SqlCommand cmd = new SqlCommand("sp_merge_vehiculo", cn);
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_vehiculo", 0);
                    cmd.Parameters.AddWithValue("@id_modelo", vehiculo.IdModelo);
                    cmd.Parameters.AddWithValue("@id_color", vehiculo.IdColor);
                    cmd.Parameters.AddWithValue("@id_transmision", vehiculo.IdTransmision);
                    cmd.Parameters.AddWithValue("@precio", vehiculo.Precio);
                    cmd.Parameters.AddWithValue("@stock", vehiculo.Stock);

                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"Registro insertado {c} de vehiculo";
                }
                ViewBag.mensaje = mensaje;

                ViewBag.modelos = new SelectList(await Task.Run(() => Modelos()), "IdModelo", "NombreModelo", vehiculo.IdModelo);
                ViewBag.transmisions = new SelectList(await Task.Run(() => Transmisions()), "IdTransmision", "NombreTransmision", vehiculo.IdTransmision);
                ViewBag.colors = new SelectList(await Task.Run(() => Colors()), "IdColor", "NombreColor", vehiculo.IdColor);

                return RedirectToAction("ListadoVehiculos");
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al guardar el vehículo: " + ex.Message;
                return View(vehiculo);
            }
           
        }

        //buscar vehiculo 
        Vehiculo Buscar(int id)
        {
            Vehiculo? vehiculo = obtenerVehiculo().Where(v => v.IdVehiculo == id).FirstOrDefault();
            return vehiculo;
        }

        //detalle vehiculo
        Vehiculo verDetalle(int id)
        {
            Vehiculo? vehiculo = Get().Where(v => v.IdVehiculo == id).FirstOrDefault();
            return vehiculo;
        }

        //Vista detalles de vehiculo
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Vehiculo vehiculo = verDetalle(id);
            if (vehiculo == null) return RedirectToAction("ListadoVehiculos");
            return View(vehiculo);
        }

        //Vista editar vehiculo
        public async Task<IActionResult> Edit(int id)
        {
            Vehiculo vehiculo = Buscar(id);
            if (vehiculo == null)
            {
                return RedirectToAction("ListadoVehiculos");
            }

            ViewBag.modelos = new SelectList(await Task.Run(() => Modelos()), "IdModelo", "NombreModelo", vehiculo.IdModelo);
            ViewBag.transmisions = new SelectList(await Task.Run(() => Transmisions()), "IdTransmision", "NombreTransmision", vehiculo.IdTransmision);
            ViewBag.colors = new SelectList(await Task.Run(() => Colors()), "IdColor", "NombreColor", vehiculo.IdColor);
            return View(vehiculo);     
        }

        //Metodo editar vehiculo
        [HttpPost]
        public async Task<IActionResult> Edit(Vehiculo vehiculo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "El modelo no es válido. Por favor, revisa los campos.";

                ViewBag.modelos = new SelectList(await Task.Run(() => Modelos()), "IdModelo", "NombreModelo", vehiculo.IdModelo);
                ViewBag.transmisions = new SelectList(await Task.Run(() => Transmisions()), "IdTransmision", "NombreTransmision", vehiculo.IdTransmision);
                ViewBag.colors = new SelectList(await Task.Run(() => Colors()), "IdColor", "NombreColor", vehiculo.IdColor);
                return View(vehiculo);
            }
            try
            {
                string mensaje = "";
                using (SqlConnection cn = new SqlConnection(configuration["ConnectionStrings:cn"]))
                {
                    SqlCommand cmd = new SqlCommand("sp_merge_vehiculo", cn);
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_vehiculo", vehiculo.IdVehiculo);
                    cmd.Parameters.AddWithValue("@id_modelo", vehiculo.IdModelo);
                    cmd.Parameters.AddWithValue("@id_color", vehiculo.IdColor);
                    cmd.Parameters.AddWithValue("@id_transmision", vehiculo.IdTransmision);
                    cmd.Parameters.AddWithValue("@precio", vehiculo.Precio);
                    cmd.Parameters.AddWithValue("@stock", vehiculo.Stock);

                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"registro actualizado {c} de vehiculo";

                }
                ViewBag.mensaje = mensaje;
                ViewBag.modelos = new SelectList(await Task.Run(() => Modelos()), "IdModelo", "NombreModelo", vehiculo.IdModelo);
                ViewBag.transmisions = new SelectList(await Task.Run(() => Transmisions()), "IdTransmision", "NombreTransmision", vehiculo.IdTransmision);
                ViewBag.colors = new SelectList(await Task.Run(() => Colors()), "IdColor", "NombreColor", vehiculo.IdColor);
                return RedirectToAction("ListadoVehiculos");
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al guardar el vehículo: " + ex.Message;
                return View(vehiculo);
            }
        }

        //Metodo eliminar
        public IActionResult Delete(int id)
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(configuration["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_eliminar_vehiculo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                cmd.Parameters.AddWithValue("@id_vehiculo", id);
                int c = cmd.ExecuteNonQuery();
                mensaje = $"registro eliminado{c}";
            }
            ViewBag.mensaje = mensaje;
            return RedirectToAction("ListadoVehiculos");
        }
    }
}
