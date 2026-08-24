using Bogus;
using Microsoft.AspNetCore.Identity;
using MediFlowApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediFlowApi.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await context.Database.EnsureCreatedAsync();

            // 1. إنشاء الأدوار (Roles) إذا لم تكن موجودة
            string[] roles = { "Doctor", "Patient", "Pharmacist" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // عدم التكرار في حال وجود بيانات سابقة (نتحقق من جدول الأطباء)
            if (context.Doctors.Any()) return;

            var faker = new Faker();
            var rand = new Random(); // نستخدم Random الأساسي من C#

            // 2. إنشاء المستخدمين وتعيين الأدوار لهم
            var doctorUsers = new List<ApplicationUser>();
            var patientUsers = new List<ApplicationUser>();
            var pharmacistUsers = new List<ApplicationUser>();

            for (int i = 1; i <= 20; i++)
            {
                var docUser = new ApplicationUser { UserName = $"doctor{i}@mediflow.com", Email = $"doctor{i}@mediflow.com", FirstName = faker.Name.FirstName(), LastName = faker.Name.LastName(), EmailConfirmed = true };
                await userManager.CreateAsync(docUser, "P@ssword123!");
                await userManager.AddToRoleAsync(docUser, "Doctor");
                doctorUsers.Add(docUser);

                var patUser = new ApplicationUser { UserName = $"patient{i}@gmail.com", Email = $"patient{i}@gmail.com", FirstName = faker.Name.FirstName(), LastName = faker.Name.LastName(), EmailConfirmed = true };
                await userManager.CreateAsync(patUser, "P@ssword123!");
                await userManager.AddToRoleAsync(patUser, "Patient");
                patientUsers.Add(patUser);

                var pharmUser = new ApplicationUser { UserName = $"pharmacist{i}@mediflow.com", Email = $"pharmacist{i}@mediflow.com", FirstName = faker.Name.FirstName(), LastName = faker.Name.LastName(), EmailConfirmed = true };
                await userManager.CreateAsync(pharmUser, "P@ssword123!");
                await userManager.AddToRoleAsync(pharmUser, "Pharmacist");
                pharmacistUsers.Add(pharmUser);
            }

            // 3. إنشاء 20 طبيب (Doctors)
            var specializations = new[] { "Cardiology", "Dermatology", "Neurology", "Pediatrics", "Orthopedics" };
            int docIndex = 0;

            var doctorFaker = new Faker<Doctor>()
                .RuleFor(d => d.Name, f => $"Dr. {f.Name.FullName()}")
                .RuleFor(d => d.Specialization, f => f.PickRandom(specializations))
                .RuleFor(d => d.LicenseNumber, f => f.Random.Number(10000, 99999))
                .RuleFor(d => d.YearOfExperience, f => f.Random.Number(1, 25))
                .RuleFor(d => d.ApplicationUserId, f => doctorUsers[docIndex++].Id);

            var doctors = doctorFaker.Generate(20);
            context.AddRange(doctors);
            await context.SaveChangesAsync();

            // 4. إنشاء 20 مريض (Patients)
            var bloodTypes = new[] { "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-" };
            int patIndex = 0;

            var patientFaker = new Faker<Patient>()
                .RuleFor(p => p.Name, f => f.Name.FullName())
                .RuleFor(p => p.age, f => f.Random.Number(18, 75))
                .RuleFor(p => p.MedicalHistory, f => f.Lorem.Sentence())
                .RuleFor(p => p.BloodType, f => f.PickRandom(bloodTypes))
                .RuleFor(p => p.ApplicatioUserId, f => patientUsers[patIndex++].Id);

            var patients = patientFaker.Generate(20);
            context.AddRange(patients);
            await context.SaveChangesAsync();

            // 5. إنشاء 20 دواء (Medicines) - المتغير معرّف هنا
            var medicineFaker = new Faker<Medicine>()
                .RuleFor(m => m.Name, f => f.Commerce.ProductName() + " " + f.Random.Number(100, 500) + "mg")
                .RuleFor(m => m.Price, f => Math.Round(f.Random.Double(15, 350), 2))
                .RuleFor(m => m.Description, f => f.Lorem.Sentence())
                .RuleFor(m => m.ExpireDate, f => f.Date.Future(2))
                .RuleFor(m => m.CreatedAt, f => DateTime.Now)
                .RuleFor(m => m.StockQuantity, f => f.Random.Number(10, 150));

            var medicines = medicineFaker.Generate(20); // القائمة تم إنشاؤها هنا
            context.AddRange(medicines);
            await context.SaveChangesAsync();

            // 6. إنشاء 20 استشارة (Consultations)
            var consultationFaker = new Faker<Consultation>()
                .RuleFor(c => c.ConsultationDate, f => f.Date.Recent(30))
                .RuleFor(c => c.Diagnoses, f => f.Lorem.Sentence())
                .RuleFor(c => c.Symptoms, f => string.Join(", ", f.Lorem.Words(3)))
                .RuleFor(c => c.IsDeleted, false);

            var consultations = new List<Consultation>();
            for (int i = 0; i < 20; i++)
            {
                var consult = consultationFaker.Generate();
                consult.DoctorId = doctors[rand.Next(doctors.Count)].Id; // ربط عشوائي بطبيب
                consult.PatientId = patients[rand.Next(patients.Count)].Id; // ربط عشوائي بمريض
                consultations.Add(consult);
            }
            context.AddRange(consultations);
            await context.SaveChangesAsync();

            // 7. إنشاء 20 روشتة (Prescriptions)
            var prescriptions = new List<Prescription>();
            for (int i = 0; i < 20; i++)
            {
                prescriptions.Add(new Prescription
                {
                    ConsultationId = consultations[i].Id,
                    DurationInDays = rand.Next(5, 30),
                    IsDispensed = i % 2 == 0 // نصفهم مصروف ونصفهم غير مصروف
                });
            }
            context.AddRange(prescriptions);
            await context.SaveChangesAsync();

            // 8. إنشاء عناصر الروشتة (PrescriptionItems)
            var prescriptionItemFaker = new Faker<PrescriptionItem>()
                .RuleFor(pi => pi.Dose, f => $"{f.Random.Number(1, 3)} tablet(s) daily")
                .RuleFor(pi => pi.Instructions, f => f.Lorem.Sentence())
                .RuleFor(pi => pi.Quantity, f => f.Random.Number(1, 3));

            var prescriptionItems = new List<PrescriptionItem>();
            foreach (var pres in prescriptions)
            {
                var item = prescriptionItemFaker.Generate();
                item.PrescriptionId = pres.Id;

                // استخدام القائمة medicines التي عرفناها في الخطوة 5
                item.MedicineId = medicines[rand.Next(medicines.Count)].Id;

                prescriptionItems.Add(item);
            }
            context.AddRange(prescriptionItems);
            await context.SaveChangesAsync();

            // 9. إنشاء سجلات الصرف (DispensingRecords) للروشتات المصروفة
            var dispensingRecords = new List<DispensingRecord>();

            foreach (var pres in prescriptions)
            {
                if (pres.IsDispensed)
                {
                    dispensingRecords.Add(new DispensingRecord
                    {
                        PrescriptionId = pres.Id,
                        PharmacistId = pharmacistUsers[rand.Next(pharmacistUsers.Count)].Id,
                        DispensedAt = DateTime.Now.AddDays(-rand.Next(1, 5)),
                        Notes = faker.Lorem.Sentence()
                    });
                }
            }
            context.AddRange(dispensingRecords);
            await context.SaveChangesAsync();
        }
    }
}