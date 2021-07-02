using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeachersUIWeb.Models
{
    public class Teacher
    {
           
            public int Id { get; set; }
       
            public string Name { get; set; }
        
            public string Skills { get; set; }
            public int TotalStudents { get; set; }
         
            public decimal Salary { get; set; }
            public DateTime AddedOn { get; set; }
        
    }
}
