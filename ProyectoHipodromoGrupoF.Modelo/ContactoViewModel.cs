using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class ContactoViewModel
    {
        public List<TelefonoPersona> TelefonosPersonas { get; set; } = new();
        public List<CorreoElectronicoPersona> CorreosPersonas { get; set; } = new();
        public List<TelefonoProveedor> TelefonosProveedores { get; set; } = new();
        public List<CorreoElectronicoProveedor> CorreosProveedores { get; set; } = new();
    }
}
