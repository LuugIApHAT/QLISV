using System;

namespace QLISV
{
    internal class SV
    {
        // Các thuộc tính với getter và setter
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string Sex { get; set; }

        // Constructor để khởi tạo đối tượng SV
        public SV(string id, string name, string address, string number, string sex)
        {
            Id = id;
            Name = name;
            Address = address;
            Number = number;
            Sex = sex;
        }

        // Phương thức hiển thị thông tin sinh viên
        public void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Number: {Number}");
            Console.WriteLine($"Sex: {Sex}");
        }
    }
}
