using System;
using System.Collections.Generic;

namespace PG_Api.Models.Entity;

public partial class User
{
    public int IdUser { get; set; }

    public string? Name { get; set; }

    public string? Pwd { get; set; }
}
