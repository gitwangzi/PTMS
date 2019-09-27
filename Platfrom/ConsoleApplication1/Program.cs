using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Ant.DBEntity;

namespace ConsoleApplication1
{
    class Program
    {


        public static void Main(string[] args)
        {
            ANTEntities _context = new ANTEntities();
            Random rand = new Random();
            int minX = -8001;
            int maxX = -7526;
            int minY = -329;
            int maxY = 43;
            string vehicle_id;
            int x, y;

            genTableVehicle();
            try
            {



                for(int i = 0; i <= 55000; i ++)
                {
                    vehicle_id = "JX" + i.ToString().PadLeft(5, '0');
                    x = rand.Next(minX, maxX);
                    y = rand.Next(minY, maxY);

                    Console.WriteLine("{0}", i);
                    VEHICLE_LOCATION rec = new VEHICLE_LOCATION()
                    {
                        ID = Guid.NewGuid().ToString(),
                        VEHICLE_ID = vehicle_id,
                        LONGITUDE = x.ToString(),
                        LATITUDE = y.ToString(),
                        GPS_TIME = DateTime.Now
                    };
                    _context.VEHICLE_LOCATION.Add(rec);
                    if (i % 200 == 0)
                    {
                        _context.SaveChanges();
                    }

                }



            }
            catch (Exception ex)
            {

            }
        }

        public static void genTableVehicle()
        {
            ANTEntities _context = new ANTEntities();
            Random rand = new Random();
            string vehicle_id;
            int type;
            try
            {
                for (int i = 10001; i <= 55000; i++)
                {
                    vehicle_id = "JX" + i.ToString().PadLeft(5, '0');
                    type = rand.Next(1, 4);
                    Console.WriteLine("{0}",i);
                    VEHICLE rec = new VEHICLE()
                    {
                        VEHICLE_ID = vehicle_id,
                        VEHICLE_TYPE = (short)type
                    };
                    _context.VEHICLE.Add(rec);

                    if (i % 200 == 0)
                    {
                        _context.SaveChanges();
                    }
                }



            }
            catch (Exception ex)
            {

            }
        }
    }
}
