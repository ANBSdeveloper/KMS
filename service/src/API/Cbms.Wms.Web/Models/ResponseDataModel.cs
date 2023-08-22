using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.Serialization;

namespace Cbms.Kms.Web.Models
{
	[DataContract]
	public class ResponseData
	{
		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public int Code { get; set; }

		[DataMember]
		public object Data { get; set; }

		[DataMember]
		public string Error { get; set; }

		public ResponseData()
		{
			this.Code = 0;
		}

		public ResponseData SuccessResponse(object data)
		{
			return new ResponseData()
			{
				Data = data,
				Code = 0
			};
		}

		public ResponseData SuccessResponse(string message)
		{
			return new ResponseData()
			{
				Code = 0,
				Message = message
			};
		}

		public ResponseData SuccessResponse(int code, string message)
		{
			return new ResponseData()
			{
				Code = code,
				Message = message
			};
		}

		public ResponseData SuccessResponse(object data, string message, int code = 0)
		{
			return new ResponseData()
			{
				Code = code,
				Data = data,
				Message = message
			};
		}

		public ResponseData SuccessResponse(string data, string message, int code = 0)
		{
			return new ResponseData()
			{
				Code = code,
				Data = data,
				Message = message
			};
		}

		public ResponseData ErrorResponse(object data)
		{
			return new ResponseData()
			{
				Data = data,
				Code = -1
			};
		}

		public ResponseData ErrorResponse(string message)
		{
			return new ResponseData()
			{
				Code = -1,
				Message = message
			};
		}

		public ResponseData ErrorResponse(string message, int Code)
		{
			return new ResponseData()
			{
				Code = Code,
				Message = message
			};
		}

		public ResponseData ErrorResponse(object data, string message, int Code = -1)
		{
			return new ResponseData()
			{
				Code = Code,
				Data = data,
				Message = message
			};
		}

		public object ModelErrorResponse(object data, string message, int Code = -1)
		{
			return new
			{
				Result = new
				{
					Code = Code,
					Data = data,
					Message = message
				},
				Success = true
			};
		}
	}

	public static class ResponseDataExt
	{
		public static ResponseData Success(object data, string message = "")
		{
			return new ResponseData()
			{
				Data = data,
				Code = 0,
				Message = message
			};
		}

		public static ResponseData Error(string message, int Code, string errorCode)
		{
			return new ResponseData()
			{
				Message = message,
				Code = Code,
				Error = errorCode
			};
		}
	}
}