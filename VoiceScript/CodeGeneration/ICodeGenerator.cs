using DiagramModel.Components;

namespace CodeGeneration
{
    public interface ICodeGenerator
    {
        void GenerateCode();
        void GenerateClassCode(Class cls, int indentation = 0);
        void GenerateFieldCode(Field field, int indentation = 1);
        void GenerateMethodCode(Method method, int indentation = 1);
        void GenerateParametersCode(Method method);
        void GenerateParameterCode(Parameter parameter, int indentation = 0);
        void GenerateExceptionCode(string exceptionName, int indentation = 2);
    }
}
