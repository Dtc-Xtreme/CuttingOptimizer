namespace CuttingOptimizer.Domain
{
    public class Plate
    {
        public Plate()
        {
            
        }

        public Plate(string id, int width, int length, int height)
        {
            ID = id;
            Width = width;
            Length = length;
            Height = height;
        }

        public string ID { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
    }
}