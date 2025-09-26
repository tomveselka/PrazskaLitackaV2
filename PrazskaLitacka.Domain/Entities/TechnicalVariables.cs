using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrazskaLitacka.Domain.Entities;
public class TechnicalVariables
{
    [Key]
    public int Id { get; set; }

    public DateTime TimeOfLastStationUpdate { get; set; } = DateTime.MinValue;
}

