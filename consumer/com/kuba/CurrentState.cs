// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.3
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace com.kuba
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("avrogen", "1.11.3")]
	public partial class CurrentState : global::Avro.Specific.ISpecificRecord
	{
		public static global::Avro.Schema _SCHEMA = global::Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"CurrentState\",\"namespace\":\"com.kuba\",\"fields\":[{\"name\":\"" +
				"Facility\",\"type\":{\"type\":\"enum\",\"name\":\"Facility\",\"namespace\":\"com.kuba\",\"symbol" +
				"s\":[\"Ord\",\"Wdr\",\"Dfw\",\"Atl\"]}}]}");
		private com.kuba.Facility _Facility;
		public virtual global::Avro.Schema Schema
		{
			get
			{
				return CurrentState._SCHEMA;
			}
		}
		public com.kuba.Facility Facility
		{
			get
			{
				return this._Facility;
			}
			set
			{
				this._Facility = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.Facility;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.Facility = (com.kuba.Facility)fieldValue; break;
			default: throw new global::Avro.AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
