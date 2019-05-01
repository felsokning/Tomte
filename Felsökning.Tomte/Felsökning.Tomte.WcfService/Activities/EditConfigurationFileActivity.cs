//-----------------------------------------------------------------------
// <copyright file="EditConfigurationFileActivity.cs" company="None">
//     Copyright (c) 2019 felsökning. All rights reserved.
// </copyright>
// <Authors>felsökning</Authors>
//-----------------------------------------------------------------------
namespace Felsökning.Tomte.WcfService.Activities
{
    using System.Activities;
    using System.Configuration;
    using System.IO;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EditConfigurationFileActivity"/> class.
    /// </summary>
    /// <inheritdoc cref="SwedishCodeActivity{T}"/>
    public class EditConfigurationFileActivity : SwedishCodeActivity<object>
    {
        /// <summary>
        ///     Gets or sets the value of the <see cref="FirstArgument"/> parameter.
        /// </summary>
        public InArgument<string> FirstArgument { get; set; }

        /// <summary>
        ///     Gets or sets the value of the <see cref="SecondArgument"/> parameter.
        /// </summary>
        public InArgument<string> SecondArgument { get; set; }

        /// <summary>
        ///     Gets or sets the value of the <see cref="ThirdArgument"/> parameter.
        /// </summary>
        public InArgument<string> ThirdArgument { get; set; }

        /// <summary>
        ///     Overrides the <see cref="Execute"/> method exposed by the <see cref="SwedishCodeActivity{T}"/> class.
        /// </summary>
        /// <param name="context">The execution context passed when invoked.</param>
        /// <returns>A string back to the caller.</returns>
        /// <inheritdoc cref="SwedishCodeActivity{T}"/>
        protected override object Execute(CodeActivityContext context)
        {
            FirstArgument = FirstInArgument;
            SecondArgument = SecondInArgument;
            ThirdArgument = ThirdInArgument;

            string configurationFile = context.GetValue(FirstArgument);
            string configurationKey = context.GetValue(SecondArgument);
            string configurationValue = context.GetValue(ThirdArgument);

            // No point in even attempting, if the file doesn't even exist.
            if (File.Exists(configurationFile))
            {
                ExeConfigurationFileMap newExeConfigurationFileMap = new ExeConfigurationFileMap(configurationFile);
                Configuration targetConfiguration = ConfigurationManager.OpenMappedExeConfiguration(newExeConfigurationFileMap, ConfigurationUserLevel.None);
                if (targetConfiguration.AppSettings.Settings[configurationKey] != null)
                {
                    targetConfiguration.AppSettings.Settings[configurationKey].Value = configurationValue;
                }
                else
                {
                    // Assume best-faith effort on the part of the operator.
                    targetConfiguration.AppSettings.Settings.Add(configurationKey, configurationValue);
                }

                targetConfiguration.Save(ConfigurationSaveMode.Modified);

                return "Completed";
            }
            else
            {
                return "Configuration file does not exist!";
            }
        }
    }
}