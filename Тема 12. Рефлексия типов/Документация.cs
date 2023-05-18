using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
	private Type type = typeof(T);
	public string GetApiDescription()
	{
		var api = type.GetCustomAttributes().OfType<ApiDescriptionAttribute>().FirstOrDefault();
		return api?.Description;
	}

	public string[] GetApiMethodNames()
	{
		return type.GetMethods()
			.Where(method => method.GetCustomAttributes().OfType<ApiMethodAttribute>().Any())
			.Select(method => method.Name).ToArray();
	}

	public string GetApiMethodDescription(string methodName)
	{
		var method = type.GetMethod(methodName);
		if (method == null || !method.GetCustomAttributes().OfType<ApiMethodAttribute>().Any())
			return null;
		var attribute = method.GetCustomAttributes().OfType<ApiDescriptionAttribute>().FirstOrDefault();
		return attribute?.Description;
	}

	public string[] GetApiMethodParamNames(string methodName)
	{
		var method = type.GetMethod(methodName);
		if (method == null || !method.GetCustomAttributes().OfType<ApiMethodAttribute>().Any())
			return null;
		return method.GetParameters()
			.Select(param => param.Name).ToArray();
	}

	public string GetApiMethodParamDescription(string methodName, string paramName)
	{
		var method = type.GetMethod(methodName);
		if (method == null || !method.GetCustomAttributes().OfType<ApiMethodAttribute>().Any())
			return null;
		var parameter = method.GetParameters().Where(param => param.Name == paramName);
		if (!parameter.Any())
			return null;
		var api = parameter.FirstOrDefault().GetCustomAttributes().OfType<ApiDescriptionAttribute>().FirstOrDefault();
		return api?.Description;
	}

	public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
	{
		var result = new ApiParamDescription { ParamDescription = new CommonDescription(paramName) };
		var method = type.GetMethod(methodName);
		if (method == null || !method.GetCustomAttributes().OfType<ApiMethodAttribute>().Any()) return result;
		var parameter = method.GetParameters().Where(param => param.Name == paramName);
		return !parameter.Any() ? result : FillFullParamsDescription(parameter, result);
	}

	private ApiParamDescription FillFullParamsDescription(IEnumerable<ParameterInfo> parameter, ApiParamDescription result)
	{
		var description = parameter.FirstOrDefault()
			.GetCustomAttributes().OfType<ApiDescriptionAttribute>()
			.FirstOrDefault();
		if (description != null)
			result.ParamDescription.Description = description.Description;
		var validationAttribute = parameter.FirstOrDefault()
			.GetCustomAttributes().OfType<ApiIntValidationAttribute>()
			.FirstOrDefault();
		if (validationAttribute != null)
		{
			result.MinValue = validationAttribute.MinValue;
			result.MaxValue = validationAttribute.MaxValue;
		}
		var requiredAttribute = parameter.First().GetCustomAttributes().OfType<ApiRequiredAttribute>().FirstOrDefault();
		if (requiredAttribute != null)
			result.Required = requiredAttribute.Required;
		return result;
	}

	public ApiMethodDescription GetApiMethodFullDescription(string methodName)
	{
            var method = type.GetMethod(methodName);
            if (method == null || !method.GetCustomAttributes().OfType<ApiMethodAttribute>().Any())
                return null;
            var result = new ApiMethodDescription
            {
	            MethodDescription = new CommonDescription(methodName, GetApiMethodDescription(methodName)),
	            ParamDescriptions = GetApiMethodParamNames(methodName)
		            .Select(param => GetApiMethodParamFullDescription(methodName, param))
		            .ToArray()
            };
            
            return FillFullMethodDescription(method.ReturnParameter, result);
	}

	private ApiMethodDescription FillFullMethodDescription(ParameterInfo returnParameter, ApiMethodDescription result)
	{
		var returnedDescription = new ApiParamDescription { ParamDescription = new CommonDescription() };
		FillDescriptionAttribute(returnParameter, returnedDescription);
		FillValidationAttribute(returnParameter, returnedDescription);
		var isReturned = FillRequiredAttribute(returnParameter, returnedDescription);
		if (isReturned)
			result.ReturnDescription = returnedDescription;
		return result;
	}

	private void FillDescriptionAttribute
		(ParameterInfo returnParameter, ApiParamDescription returnedDescription)
	{
		var descriptionAttribute = returnParameter
			.GetCustomAttributes().OfType<ApiDescriptionAttribute>().FirstOrDefault();
		if (descriptionAttribute != null)
			returnedDescription.ParamDescription.Description = descriptionAttribute.Description;
	}

	private void FillValidationAttribute
		(ParameterInfo returnParameter, ApiParamDescription returnedDescription)
	{
		var validationAttribute = returnParameter.GetCustomAttributes()
			.OfType<ApiIntValidationAttribute>().FirstOrDefault();
		if (validationAttribute != null)
		{
			returnedDescription.MinValue = validationAttribute.MinValue;
			returnedDescription.MaxValue = validationAttribute.MaxValue;
		}
	}

	private bool FillRequiredAttribute
		(ParameterInfo returnParameter, ApiParamDescription returnedDescription)
	{
		var requiredAttribute = returnParameter
			.GetCustomAttributes().OfType<ApiRequiredAttribute>().FirstOrDefault();
		if (requiredAttribute != null)
		{
			returnedDescription.Required = requiredAttribute.Required;
			return true;
		}
		return false;
	}
}