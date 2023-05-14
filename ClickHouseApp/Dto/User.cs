using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickHouseApp.Dto
{
    public class User
    {
        public int UserId { get; set; }

        public string? UserName { get; set; }

        public double? Weight { get; set; }
    }
}
