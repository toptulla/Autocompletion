namespace Autocompletion.Domain
{
    /// <summary>
    /// Поведеине источника данных
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// Возвращает объект, представляющий слова и автодополнения
        /// </summary>
        /// <returns>Объект-контейнер для слов и автодополнений</returns>
        InputData GetInputData();
    }
}