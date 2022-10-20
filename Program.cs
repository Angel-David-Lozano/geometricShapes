using System.Text;

namespace Reader
{
    class GeometricProperties
    {   
        string[] Polygon(List<double> x, List<double> y)
        {
            double Area = 0;
            double Perimeter = 0;
            double Centroidx = 0;
            double Centroidy = 0;

            for (int i = 0; i < x.Count - 1; i++)
            {
                double xi = x[i];
                double yi = y[i];
                double xj = x[i + 1];
                double yj = y[i + 1];

                Area += (xi * yj - xj * yi)/2;
                Perimeter += Math.Sqrt(Math.Pow(xj - xi, 2) + Math.Pow(yj - yi, 2));

                Centroidx += (xi + xj) * (xi * yj - xj * yi);
                Centroidy += (yi + yj) * (xi * yj - xj * yi);
            }
            Centroidx = Centroidx / (6 * Area);
            Centroidy = Centroidy / (6 * Area);
              
            return new string[] { "Polygon", "Area",Area.ToString(), "Perimeter", Perimeter.ToString(), 
                                "Centroidx", Centroidx.ToString(), "Centroidy", Centroidy.ToString() };     
        }
        public string[] Ellipse (double radiuos1, double radiuos2, double centerx, double centery)
        {
            double Area = Math.PI * radiuos1 * radiuos2;

            //Ramanujan's approximation
            double Perimeter = Math.PI * (3 * (radiuos1 + radiuos2) - Math.Sqrt((3 * radiuos1 + radiuos2) * (radiuos1 + 3 * radiuos2)));
               
            return new string[] { "Ellipse", "Area", Area.ToString(), "Perimeter", Perimeter.ToString(), 
                                "Centroidx", centerx.ToString(), "Centroidy", centery.ToString() }; 
        }
        public string[] Square(double side, double centerx, double centery)
        {
            double Area = side * side;
            double Perimeter = 4 * side;
            
            return new string[] { "Square", "Area",Area.ToString(), "Perimeter", Perimeter.ToString(), 
                                "Centroidx", centerx.ToString(), "Centroidy", centery.ToString() }; 
        }
        
        public string[] Circule(double radius, double centerx, double centery)
        {                     
            double Area = Math.PI * Math.Pow(radius, 2);
            double Perimeter = 2 * Math.PI * radius;
            
            return new string[] { "Circule", "Area",Area.ToString(), "Perimeter", Perimeter.ToString(), 
                                "Centroidx", centerx.ToString(), "Centroidy", centery.ToString() }; 
        }

        string[] EquilateralTriangle(double side, double centerx, double centery)
        {
            double Area = Math.Pow(side, 2) * Math.Sqrt(3) / 4;
            double Perimeter = 3 * side;

            return new string[] { "Equilateral Triangle", "Area",Area.ToString(), "Perimeter", Perimeter.ToString(), 
                                "Centroidx", centerx.ToString(), "Centroidy", centery.ToString() };  
        }
        
        public string[] clasifyShape(string[] rowData)
        {         
            string ID = rowData[1];

            if (ID == "Circle")
            {
                double radius = double.Parse(rowData[7]);
                double centerx = double.Parse(rowData[3]);
                double centery = double.Parse(rowData[5]);
                return Circule(radius, centerx, centery);
           
            }
            else if (ID == "Equilateral Triangle")
            {
                double side = double.Parse(rowData[7]);
                double centerx = double.Parse(rowData[3]);
                double centery = double.Parse(rowData[5]);
                return EquilateralTriangle(side, centerx, centery);
                
            }
            else if (ID == "Square")
            {
                double side = double.Parse(rowData[7]);
                double centerx = double.Parse(rowData[3]);
                double centery = double.Parse(rowData[5]);
                return Square(side, centerx, centery);
            }
            else if (ID == "Ellipse")
            {
                double radius1 = double.Parse(rowData[7]);
                double radius2 = double.Parse(rowData[9]);
                double centerx = double.Parse(rowData[3]);
                double centery = double.Parse(rowData[5]);
                return Ellipse(radius1, radius2, centerx, centery);
            }
            else if (ID == "Polygon")
            {
                List <double> xvalues = new List<double>();
                List <double> yvalues = new List<double>();
                for (int i = 3; i < rowData.Length; i+=4)
                {
                    double.TryParse(rowData[i], out double x);
                    double.TryParse(rowData[i+2], out double y);
                    if (x != 0)
                    {
                        xvalues.Add(x);
                        yvalues.Add(y);
                    }                  
                }
                return Polygon(xvalues, yvalues);
            }
            else
            {
                return new string[] { "Not a valid Shape" };
            }           
        }
    }

    class Program
    {   static void Main(string[] args)
        {
            string[] data = File.ReadAllLines("ShapeList2.csv");
            var saveCSV = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                string[] row = data[i].Split(',');
                GeometricProperties gp = new GeometricProperties();
                var csvrows = gp.clasifyShape(row);
                saveCSV.AppendLine(string.Join(",", csvrows));
            }
            File.WriteAllText("Result.csv", saveCSV.ToString());
        }
    }
}
