using System;
using System.IO;
using System.Collections.Generic;

namespace ntregsharp
{
	public class ValueKey
	{
		public ValueKey (BinaryReader hive)
		{
			this.AbsoluteOffset = hive.BaseStream.Position;

			byte[] buf = hive.ReadBytes(2);
			
			if (buf[0] != 0x76 && buf[1] != 0x6b)
				throw new NotSupportedException("Bad vk header");
			
			buf = hive.ReadBytes(2);
			
			this.NameLength = BitConverter.ToInt16(buf,0);
			
			this.DataLength = BitConverter.ToInt32(hive.ReadBytes(4),0);
			
			//dataoffset, unless data is stored here
			byte[] databuf = hive.ReadBytes(4);
			
			this.ValueType = hive.ReadInt32();

			hive.BaseStream.Position += 4;
			
			buf = hive.ReadBytes(this.NameLength);
			this.Name = (this.NameLength == 0) ? "Default" : System.Text.Encoding.UTF8.GetString(buf);

			if (this.DataLength < 5) {
				this.DataOffset = this.AbsoluteOffset + 8;
				this.Data = databuf;
			}
			else
			{
				hive.BaseStream.Position = 0x1000 + BitConverter.ToInt32(databuf, 0) + 0x04;
				this.DataOffset = hive.BaseStream.Position;
				this.Data = hive.ReadBytes(this.DataLength);
				if (this.ValueType == 1)
					this.String = System.Text.Encoding.Unicode.GetString (this.Data);
			}
		}

		public void EditName(FileStream hive, string newName){
			byte[] name = System.Text.Encoding.UTF8.GetBytes(System.Text.Encoding.UTF8.GetString(System.Text.Encoding.ASCII.GetBytes (newName)));
			if (name.Length > this.NameLength)
				throw new Exception ("New name cannot be longer than old name currently.");

			hive.Position = this.AbsoluteOffset + 20;

			int k = this.NameLength - name.Length;
			hive.Write (name, 0, name.Length);

			for (int i = 0; i < k; i++)
				hive.WriteByte (0x00);

		}

		public void EditData(FileStream hive, byte[] data, int valueType) {
			if (data.Length > this.DataLength)
				throw new Exception ("New data cannot be longer than old data currently.");

			hive.Position = this.DataOffset;

			int k = this.DataLength - data.Length;
			hive.Write (data, 0, data.Length);

			for (int i = 0; i < k; i++)
				hive.WriteByte (0x00);
		}

		public int AbsoluteOffset { get; set; }
		public short NameLength { get; set; }
		public int DataLength { get; set; }
		public int DataOffset { get; set; }
		public int ValueType { get; set; }
		public string Name { get; set; }
		public byte[] Data { get; set; }
		public string String { get; set; }
	}
}

