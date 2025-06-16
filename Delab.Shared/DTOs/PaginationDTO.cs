using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.Shared.DTOs;

public class PaginationDTO
{
    public int Id { get; set; }

    public int Page { get; set; } = 1;

    public int RecordsNumber { get; set; } = 15;  //Numero de registros que se muestran por paginacion

    public string? Filter { get; set; }

    public string? DateStart { get; set; }

    public string? DateEnd { get; set; }
}
