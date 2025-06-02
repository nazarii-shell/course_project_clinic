

using DataAccess.Models;

namespace BussinessLogic.Models;

public class AppointmentBusinessModel
{
    public Doctor Doctor { get; set; }

    public Patient Patient { get; set; }

    public DateTime AppointmentDate { get; set; }
    public string Reason { get; set; }
}

//
// // register
// app.MapPost("/register", () =>
// {
//     return "registered";
// });
//
// // add doctor
// app.MapPost("/doctor", () =>
// {
//     return "new doctor";
// });
//
// // add patient
// app.MapPost("/patient", () =>
// {
//     return "new patient";
// });
//
// // search for doctors, use filters
// app.MapGet("/doctors", () =>
// {
//     return "new doctor";
// });
//
// // book appointment is registered, otherwise see info about appointments
// app.MapPost("/appointment", () =>
// {
//     return "booked";
// });
