using System.ComponentModel;

namespace Samr.ERP.Infrastructure.Enums
{
	public enum Days
	{
		[Description("Вс")]
		Sunday = 0,
		//
		// Summary:
		//     Indicates Monday.
		[Description("Пн")]
		Monday = 1,
		//
		// Summary:
		//     Indicates Tuesday.
		[Description("Вт")]
		Tuesday = 2,
		//
		// Summary:
		//     Indicates Wednesday.
		[Description("Ср")]
		Wednesday = 3,
		//
		// Summary:
		//     Indicates Thursday.
		[Description("Чт")]
		Thursday = 4,
		//
		// Summary:
		//     Indicates Friday.
		[Description("Пт")]
		Friday = 5,
		//
		// Summary:
		//     Indicates Saturday.
		[Description("Сб")]
		Saturday = 6,
	}

}
