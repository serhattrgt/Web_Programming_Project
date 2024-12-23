using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web_Programming_Proje.Models;

namespace Web_Programming_Proje.Data{
    public static class SeedData{
       public static void EnsurePopulated(IApplicationBuilder app){

        StoreDbContext context = app.ApplicationServices
            .CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();


        if(context.Database.GetPendingMigrations().Any()){
            context.Database.Migrate();
        }



        if(!context.Roles.Any()){
            context.Roles.AddRange(
                 new Role{
                    RoleName = "Admin"
                },
                new Role{
                    RoleName = "Seller"
                },
                new Role{
                    RoleName = "Customer",
                }
            );
            context.SaveChanges();
        }



        if(!context.Categories.Any()){
            context.Categories.AddRange(
                new Category{
                    CategoryName = "Sport Cars"
                },
                new Category{
                    CategoryName = "Electric Cars"
                },
                new Category{
                    CategoryName = "Motorcycles"
                }
            );
            context.SaveChanges();
        }

        

        if(!context.Payments.Any()){
            context.Payments.Add(
                new Payment{
                    PaymentType = "Credit Card"
                }
            );
            context.SaveChanges();
        }


        if (!context.Addresses.Any())
        {
            context.Addresses.AddRange(
                new Address
                {
                    OpenAddress = "Igdır, Merkez, Cumhuriyet Mahallesi, Atatürk Caddesi No:45, 76000",
                },
                new Address
                {
                    OpenAddress = "Trabzon, Ortahisar, Atatürk Caddesi No:12, Uzun Sokak, 61000",
                },
                new Address
                {
                    OpenAddress = "İzmir, Konak, Kordonboyu, Gündoğdu Meydanı, No:8, 35260",
                },
                new Address
                {
                    OpenAddress = "Uşak, Merkez, 1. Sokak, Atatürk Bulvarı No:7, 64000",
                }
            );
            context.SaveChanges();
        }




        Role? adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");

        Address? igdir = context.Addresses.FirstOrDefault(r => r.AddressID == 1);
        Address? trabzon = context.Addresses.FirstOrDefault(r => r.AddressID == 2);
        Address? usak = context.Addresses.FirstOrDefault(r => r.AddressID == 4);
        Address? izmir = context.Addresses.FirstOrDefault(r => r.AddressID == 3);

        if (!context.Users.Any())
        {
            if (adminRole != null)
            {
                var passwordCoder = new PasswordHasher<object>(); // PASSWORDLARI VERI TABANIMIZA SIFRELENMIS BIR SEKILDE KAYDETTIM
                context.Users.AddRange(
                    new User
                    {
                        UserName = "SerhatTurgut",
                        Name = "Serhat",
                        Surname = "Turgut",
                        Password = passwordCoder.HashPassword(null!,"Serhatturgut1$"),
                        Roles = new List<Role> { adminRole },
                        Address = usak!
                    },
                    new User{
                        UserName = "YusufOkur",
                        Name = "Yusuf",
                        Surname = "Okur",
                        Password = passwordCoder.HashPassword(null!,"Yusufokur1$"),
                        Roles = new List<Role> { adminRole },
                        Address = izmir!
                    },
                     new User{
                        UserName = "YasinSebelek",
                        Name = "Yasin",
                        Surname = "Sebelek",
                        Password = passwordCoder.HashPassword(null!,"Yasinsebelek1$"),
                        Roles = new List<Role> { adminRole }, 
                        Address = trabzon!
                    },
                     new User{
                        UserName = "FatihBozkurt",
                        Name = "Fatih",
                        Surname = "Bozkurt",
                        Password = passwordCoder.HashPassword(null!,"Fatihbozkurt1$"),
                        Roles = new List<Role> { adminRole },
                        Address = igdir!
                    }
                );
                context.SaveChanges();
            }
        }
        Category? SportCar = context.Categories.FirstOrDefault(r => r.CategoryID == 1);
        Category? ElectricCars = context.Categories.FirstOrDefault(r => r.CategoryID == 2);
        Category? MotorCycles = context.Categories.FirstOrDefault(r => r.CategoryID == 3);

        if (!context.Products.Any())
        {
                context.Products.AddRange(
                    new Product{
                        ProductName = "BMW M5",
                        Brand = "BMW",
                        Model = "M5",
                        Price = 3000000,
                        StockAmount = 12,
                        Color = "Red",
                        TopSpeed = 320,
                        FuelConsume = 4.4,
                        Description = "The BMW M5 Competition is a high-performance sedan that blends luxury and power seamlessly. Equipped with a 4.4L V8 twin-turbo engine, it roars with 617 horsepower and accelerates from 0-100 km/h in just 3.3 seconds.Its bold exterior design features sharp lines, an aggressive kidney grille, and signature laser headlights. The interior offers premium Merino leather seats, a customizable digital cockpit, and a state-of-the-art iDrive infotainment system.With advanced features like M xDrive all-wheel drive, adaptive suspension, and track-ready brakes, the M5 Competition delivers an unparalleled driving experience, whether on the highway or the racetrack. Luxury, performance, and style—this is the BMW M5 Competition. 🏁",
                        Category = SportCar!,
                        Image = "/uploads/1.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "Audi RS7",
                        Brand = "Audi",
                        Model = "RS7",
                        Price = 5000000,
                        StockAmount = 5,
                        Color = "Grey",
                        TopSpeed = 350,
                        FuelConsume = 5.2,
                        Description = "The Audi RS7 Sportback is a masterpiece of engineering, combining breathtaking performance with sophisticated design. Under the hood lies a 4.0L V8 twin-turbo engine with 591 horsepower, propelling the RS7 from 0-100 km/h in an exhilarating 3.5 seconds. Its sleek Sportback silhouette, aggressive matrix LED headlights, and signature gloss-black honeycomb grille give it an unmistakable presence on the road. The wide stance and optional 22-inch alloy wheels scream confidence and power. Inside, the RS7 boasts Valcona leather seats, a fully digital Virtual Cockpit, and a dual-touchscreen MMI infotainment system. It also features cutting-edge tech like quattro all-wheel drive, adaptive air suspension, and customizable drive modes, making it a joy to drive in any condition. The Audi RS7 Sportback is where performance and luxury collide, offering an unrivaled driving experience. 🚀",
                        Category = SportCar!,
                        Image = "/uploads/2.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "Mercedes AMG E63",
                        Brand = "Mercedes",
                        Model = "AMG E63",
                        Price = 4000000,
                        StockAmount = 7,
                        Color = "Grey",
                        TopSpeed = 310,
                        FuelConsume = 5.0,
                        Description = "The Mercedes-AMG E63 S combines luxury, performance, and advanced technology into a powerhouse sedan. Powered by a 4.0L V8 twin-turbo engine, it delivers an astounding 603 horsepower and sprints from 0-100 km/h in just 3.4 seconds. Its aggressive yet elegant design features AMG Panamericana grille, sleek LED headlights, and dynamic lines that enhance its aerodynamic profile. The quad exhaust tips and 19-inch AMG wheels leave no doubt about its performance heritage. Inside, the E63 S offers Nappa leather upholstery, an advanced MBUX infotainment system, and customizable ambient lighting. With features like AMG Performance 4MATIC+ all-wheel drive, adaptive suspension, and Drift Mode, this sedan is equally at home on the track or the highway. The AMG E63 S is the epitome of refined aggression—a true beast in luxury attire. 🔥",
                        Category = SportCar!,
                        Image = "/uploads/3.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "Porsche 911 Turbo S",
                        Brand = "Porsche",
                        Model = "911 Turbo S",
                        Price = 6000000,
                        StockAmount = 2,
                        Color = "Grey",
                        TopSpeed = 370,
                        FuelConsume = 7.2,
                        Description = "The Porsche 911 Turbo S represents the pinnacle of engineering and design. Featuring a 3.8L twin-turbocharged flat-six engine with 640 horsepower, it rockets from 0-100 km/h in just 2.7 seconds, reaching a top speed of 370 km/h. Its sleek, aerodynamic body enhances performance while maintaining the iconic 911 silhouette. Inside, you'll find a perfect blend of luxury and cutting-edge technology, with premium leather seats and an advanced infotainment system. The Turbo S is a perfect harmony of speed, elegance, and heritage.",
                        Category = SportCar!,
                        Image = "/uploads/4.jpg" // Kaydedilen dosyanın yolu
                    },
                     new Product{
                        ProductName = "Lamborghini Huracan EVO ",
                        Brand = "Lamborghini",
                        Model = "Huracan EVO",
                        Price = 13000000,
                        StockAmount = 1,
                        Color = "Yellow",
                        TopSpeed = 430,
                        FuelConsume = 10.2,
                        Description = "The Lamborghini Huracán EVO is a masterpiece of Italian craftsmanship and innovation. Powered by a 5.2L naturally aspirated V10 engine, it produces 630 horsepower and accelerates from 0-100 km/h in just 2.9 seconds. Its sharp, angular design and vibrant Yellow exterior make a bold statement. The advanced Lamborghini Dinamica Veicolo Integrata (LDVI) system ensures unparalleled handling and agility, while the luxurious interior combines cutting-edge technology with bespoke materials. A true supercar for those who crave adrenaline.",
                        Category = SportCar!,
                        Image = "/uploads/5.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "Ferrari Laferrari ",
                        Brand = "Ferrari",
                        Model = "Laferrari",
                        Price = 10000000,
                        StockAmount = 7,
                        Color = "Black",
                        TopSpeed = 410,
                        FuelConsume = 9.2,
                        Description = "The Ferrari LaFerrari is the ultimate expression of Ferrari's innovation and legacy. Combining a 6.3L V12 engine with an electric motor, it delivers a staggering 950 horsepower, making it one of the most powerful hybrid supercars ever built. With a top speed of 410 km/h, this car redefines speed and performance. Its futuristic design, with aggressive lines and a low stance, reflects its racing DNA. Inside, the cockpit is designed for pure driving pleasure, featuring carbon fiber accents and cutting-edge technology. The LaFerrari is a car that turns dreams into reality.",
                        Category = SportCar!,
                        Image = "/uploads/6.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "Ducati Panigale V4",
                        Brand = "Ducati",
                        Model = "Panigale V4",
                        Price = 3000000,
                        StockAmount = 12,
                        Color = "Black",
                        TopSpeed = 320,
                        FuelConsume = 5.4,
                        Description = "The BMW M5 Competition is a high-performance sedan that blends luxury and power seamlessly. Equipped with a 4.4L V8 twin-turbo engine, it roars with 617 horsepower and accelerates from 0-100 km/h in just 3.3 seconds.Its bold exterior design features sharp lines, an aggressive kidney grille, and signature laser headlights. The interior offers premium Merino leather seats, a customizable digital cockpit, and a state-of-the-art iDrive infotainment system.With advanced features like M xDrive all-wheel drive, adaptive suspension, and track-ready brakes, the M5 Competition delivers an unparalleled driving experience, whether on the highway or the racetrack. Luxury, performance, and style—this is the BMW M5 Competition. 🏁",
                        Category = MotorCycles!,
                        Image = "/uploads/7.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "Yamaha YZF-R1",
                        Brand = "Yamaha",
                        Model = "YZF-R1",
                        Price = 5000000,
                        StockAmount = 8,
                        Color = "Dark Blue",
                        TopSpeed = 450,
                        FuelConsume = 8.2,
                        Description = "The Audi RS7 Sportback is a masterpiece of engineering, combining breathtaking performance with sophisticated design. Under the hood lies a 4.0L V8 twin-turbo engine with 591 horsepower, propelling the RS7 from 0-100 km/h in an exhilarating 3.5 seconds. Its sleek Sportback silhouette, aggressive matrix LED headlights, and signature gloss-black honeycomb grille give it an unmistakable presence on the road. The wide stance and optional 22-inch alloy wheels scream confidence and power. Inside, the RS7 boasts Valcona leather seats, a fully digital Virtual Cockpit, and a dual-touchscreen MMI infotainment system. It also features cutting-edge tech like quattro all-wheel drive, adaptive air suspension, and customizable drive modes, making it a joy to drive in any condition. The Audi RS7 Sportback is where performance and luxury collide, offering an unrivaled driving experience. 🚀",
                        Category = MotorCycles!,
                        Image = "/uploads/8.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "Honda CBR600RR",
                        Brand = "Honda",
                        Model = "CBR600RR",
                        Price = 7000000,
                        StockAmount = 18,
                        Color = "Red",
                        TopSpeed = 430,
                        FuelConsume = 9.8,
                        Description = "The Mercedes-AMG E63 S combines luxury, performance, and advanced technology into a powerhouse sedan. Powered by a 4.0L V8 twin-turbo engine, it delivers an astounding 603 horsepower and sprints from 0-100 km/h in just 3.4 seconds. Its aggressive yet elegant design features AMG Panamericana grille, sleek LED headlights, and dynamic lines that enhance its aerodynamic profile. The quad exhaust tips and 19-inch AMG wheels leave no doubt about its performance heritage. Inside, the E63 S offers Nappa leather upholstery, an advanced MBUX infotainment system, and customizable ambient lighting. With features like AMG Performance 4MATIC+ all-wheel drive, adaptive suspension, and Drift Mode, this sedan is equally at home on the track or the highway. The AMG E63 S is the epitome of refined aggression—a true beast in luxury attire. 🔥",
                        Category = MotorCycles!,
                        Image = "/uploads/9.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "Suzuki GSX-R1000",
                        Brand = "Suzuki",
                        Model = "GSX-R1000",
                        Price = 6000000,
                        StockAmount = 22,
                        Color = "Black",
                        TopSpeed = 370,
                        FuelConsume = 7.2,
                        Description = "The Porsche 911 Turbo S represents the pinnacle of engineering and design. Featuring a 3.8L twin-turbocharged flat-six engine with 640 horsepower, it rockets from 0-100 km/h in just 2.7 seconds, reaching a top speed of 370 km/h. Its sleek, aerodynamic body enhances performance while maintaining the iconic 911 silhouette. Inside, you'll find a perfect blend of luxury and cutting-edge technology, with premium leather seats and an advanced infotainment system. The Turbo S is a perfect harmony of speed, elegance, and heritage.",
                        Category = MotorCycles!,
                        Image = "/uploads/10.jpg" // Kaydedilen dosyanın yolu
                    },
                     new Product{
                        ProductName = "BMW S1000RR",
                        Brand = "BMW",
                        Model = "S1000RR",
                        Price = 13000000,
                        StockAmount = 7,
                        Color = "Black",
                        TopSpeed = 430,
                        FuelConsume = 10.2,
                        Description = "The Lamborghini Huracán EVO is a masterpiece of Italian craftsmanship and innovation. Powered by a 5.2L naturally aspirated V10 engine, it produces 630 horsepower and accelerates from 0-100 km/h in just 2.9 seconds. Its sharp, angular design and vibrant Yellow exterior make a bold statement. The advanced Lamborghini Dinamica Veicolo Integrata (LDVI) system ensures unparalleled handling and agility, while the luxurious interior combines cutting-edge technology with bespoke materials. A true supercar for those who crave adrenaline.",
                        Category = MotorCycles!,
                        Image = "/uploads/11.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "Kawasaki Ninja ZX-10R",
                        Brand = "Kawasaki",
                        Model = "Ninja ZX-10R",
                        Price = 10000000,
                        StockAmount = 7,
                        Color = "Black-Yellow",
                        TopSpeed = 410,
                        FuelConsume = 9.2,
                        Description = "The Ferrari LaFerrari is the ultimate expression of Ferrari's innovation and legacy. Combining a 6.3L V12 engine with an electric motor, it delivers a staggering 950 horsepower, making it one of the most powerful hybrid supercars ever built. With a top speed of 410 km/h, this car redefines speed and performance. Its futuristic design, with aggressive lines and a low stance, reflects its racing DNA. Inside, the cockpit is designed for pure driving pleasure, featuring carbon fiber accents and cutting-edge technology. The LaFerrari is a car that turns dreams into reality.",
                        Category = MotorCycles!,
                        Image = "/uploads/12.jpg" // Kaydedilen dosyanın yolu
                    },new Product{
                        ProductName = "Audi e-tron GT",
                        Brand = "Audi",
                        Model = "e-tron GT",
                        Price = 10000000,
                        StockAmount = 15,
                        Color = "Black",
                        TopSpeed = 330,
                        FuelConsume = 22,
                        Description = "The Ferrari LaFerrari is the ultimate expression of Ferrari's innovation and legacy. Combining a 6.3L V12 engine with an electric motor, it delivers a staggering 950 horsepower, making it one of the most powerful hybrid supercars ever built. With a top speed of 410 km/h, this car redefines speed and performance. Its futuristic design, with aggressive lines and a low stance, reflects its racing DNA. Inside, the cockpit is designed for pure driving pleasure, featuring carbon fiber accents and cutting-edge technology. The LaFerrari is a car that turns dreams into reality.",
                        Category = ElectricCars!,
                        Image = "/uploads/13.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "Tesla Model S",
                        Brand = "Tesla",
                        Model = "Model S",
                        Price = 7000000,
                        StockAmount = 12,
                        Color = "Red",
                        TopSpeed = 320,
                        FuelConsume = 20,
                        Description = "The BMW M5 Competition is a high-performance sedan that blends luxury and power seamlessly. Equipped with a 4.4L V8 twin-turbo engine, it roars with 617 horsepower and accelerates from 0-100 km/h in just 3.3 seconds.Its bold exterior design features sharp lines, an aggressive kidney grille, and signature laser headlights. The interior offers premium Merino leather seats, a customizable digital cockpit, and a state-of-the-art iDrive infotainment system.With advanced features like M xDrive all-wheel drive, adaptive suspension, and track-ready brakes, the M5 Competition delivers an unparalleled driving experience, whether on the highway or the racetrack. Luxury, performance, and style—this is the BMW M5 Competition. 🏁",
                        Category = ElectricCars!,
                        Image = "/uploads/14.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "Nissan Ariya",
                        Brand = "Nissan",
                        Model = "Ariya",
                        Price = 5000000,
                        StockAmount = 8,
                        Color = "Brown",
                        TopSpeed = 230,
                        FuelConsume = 15,
                        Description = "The Audi RS7 Sportback is a masterpiece of engineering, combining breathtaking performance with sophisticated design. Under the hood lies a 4.0L V8 twin-turbo engine with 591 horsepower, propelling the RS7 from 0-100 km/h in an exhilarating 3.5 seconds. Its sleek Sportback silhouette, aggressive matrix LED headlights, and signature gloss-black honeycomb grille give it an unmistakable presence on the road. The wide stance and optional 22-inch alloy wheels scream confidence and power. Inside, the RS7 boasts Valcona leather seats, a fully digital Virtual Cockpit, and a dual-touchscreen MMI infotainment system. It also features cutting-edge tech like quattro all-wheel drive, adaptive air suspension, and customizable drive modes, making it a joy to drive in any condition. The Audi RS7 Sportback is where performance and luxury collide, offering an unrivaled driving experience. 🚀",
                        Category = ElectricCars!,
                        Image = "/uploads/15.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "BMW i4",
                        Brand = "BMW",
                        Model = "i4",
                        Price = 7000000,
                        StockAmount = 18,
                        Color = "Black",
                        TopSpeed = 290,
                        FuelConsume = 18,
                        Description = "The Mercedes-AMG E63 S combines luxury, performance, and advanced technology into a powerhouse sedan. Powered by a 4.0L V8 twin-turbo engine, it delivers an astounding 603 horsepower and sprints from 0-100 km/h in just 3.4 seconds. Its aggressive yet elegant design features AMG Panamericana grille, sleek LED headlights, and dynamic lines that enhance its aerodynamic profile. The quad exhaust tips and 19-inch AMG wheels leave no doubt about its performance heritage. Inside, the E63 S offers Nappa leather upholstery, an advanced MBUX infotainment system, and customizable ambient lighting. With features like AMG Performance 4MATIC+ all-wheel drive, adaptive suspension, and Drift Mode, this sedan is equally at home on the track or the highway. The AMG E63 S is the epitome of refined aggression—a true beast in luxury attire. 🔥",
                        Category = ElectricCars!,
                        Image = "/uploads/16.jpg" // Kaydedilen dosyanın yolu
                    },
                    new Product{
                        ProductName = "Volkswagen ID.4",
                        Brand = "Volkswagen",
                        Model = "ID.4",
                        Price = 6000000,
                        StockAmount = 22,
                        Color = "Blue",
                        TopSpeed = 190,
                        FuelConsume = 16,
                        Description = "The Porsche 911 Turbo S represents the pinnacle of engineering and design. Featuring a 3.8L twin-turbocharged flat-six engine with 640 horsepower, it rockets from 0-100 km/h in just 2.7 seconds, reaching a top speed of 370 km/h. Its sleek, aerodynamic body enhances performance while maintaining the iconic 911 silhouette. Inside, you'll find a perfect blend of luxury and cutting-edge technology, with premium leather seats and an advanced infotainment system. The Turbo S is a perfect harmony of speed, elegance, and heritage.",
                        Category = ElectricCars!,
                        Image = "/uploads/17.jpg" // Kaydedilen dosyanın yolu
                    },
                     new Product{
                        ProductName = "Ford Mustang Mach-E ",
                        Brand = "Ford",
                        Model = "Mustang Mach-E ",
                        Price = 13000000,
                        StockAmount = 18,
                        Color = "Red",
                        TopSpeed = 220,
                        FuelConsume = 13,
                        Description = "The Lamborghini Huracán EVO is a masterpiece of Italian craftsmanship and innovation. Powered by a 5.2L naturally aspirated V10 engine, it produces 630 horsepower and accelerates from 0-100 km/h in just 2.9 seconds. Its sharp, angular design and vibrant Yellow exterior make a bold statement. The advanced Lamborghini Dinamica Veicolo Integrata (LDVI) system ensures unparalleled handling and agility, while the luxurious interior combines cutting-edge technology with bespoke materials. A true supercar for those who crave adrenaline.",
                        Category = ElectricCars!,
                        Image = "/uploads/18.jpg" // Kaydedilen dosyanın yolu
                    }
                    
                    
                );
                context.SaveChanges();
            }
        
       

       
       }
    }
}