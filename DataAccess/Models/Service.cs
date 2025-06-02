using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models;


[Table("Services")]
public class Service
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public List<Doctor> Doctors { get; set; }
}