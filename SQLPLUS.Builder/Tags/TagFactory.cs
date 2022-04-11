using SQLPLUS.Builder.ConfigurationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SQLPLUS.Builder.Tags
{
    public class TagFactory
    {
        private static readonly Dictionary<string, TagContexts> tagContextTypes = new Dictionary<string, TagContexts>(StringComparer.OrdinalIgnoreCase);

        public TagFactory (BuildDefinition buildDefinition)
        {
            PrimaryTagPrefix = $"{buildDefinition.PrimaryTagPrefix}";
            SupplementalTagPrefix = $"{buildDefinition.SupplementalTagPrefix}";
            
            CommentTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Comment).ToLower()}";
            CreditCardTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(CreditCard).ToLower()}";
            CurrencyTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Currency).ToLower()}";
            DefaultTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Default).ToLower()}";
            DisplayTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Display).ToLower()}";
            EmailTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Email).ToLower()}";
            EnumTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Enum).ToLower()}";
            ExplicitTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Explicit).ToLower()}";
            ForceColumnEncryptionTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(ForceColumnEncryption).ToLower()}";
            HtmlTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Html).ToLower()}";
            IgnoreTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Ignore).ToLower()}";
            InputTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Input).ToLower()}";
            InOutTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(InOut).ToLower()}";
            MaxlengthTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(MaxLength).ToLower()}";
            MinLengthTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(MinLength).ToLower()}";
            OutputTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Output).ToLower()}";
            ParametersTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Tags.Parameters).ToLower()}";
            PasswordTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Password).ToLower()}";
            PhoneTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Phone).ToLower()}";
            PostalCodeTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(PostalCode).ToLower()}";
            RangeTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Range).ToLower()}";
            RegExPatternTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(RegExPattern).ToLower()}";
            ReturnTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Return).ToLower()}";
            RequiredTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Required).ToLower()}";
            RoutineTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(SQLPlusRoutine).ToLower()}";
            StringLengthTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(StringLength).ToLower()}";
            QueryStartTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(QueryStart).ToLower()}";
            QueryEndTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(QueryEnd).ToLower()}";
            UrlTag = $"{buildDefinition.PrimaryTagPrefix}{nameof(Url).ToLower()}";

        //    private static readonly Dictionary<string, TagContextTypes> tagContextTypes = new Dictionary<string, TagContextTypes>(StringComparer.OrdinalIgnoreCase)
        //{
        //    { CreditCardToken, TagContextTypes.ParameterAnnotation },
        //    { CommentToken, TagContextTypes.ParameterAnnotation },
        //    { CurrencyToken, TagContextTypes.ParameterAnnotation },
        //    { DefaultToken, TagContextTypes.ParameterAnnotation },
        //    { DisplayToken, TagContextTypes.ParameterAnnotation },
        //    { EmailToken, TagContextTypes.ParameterAnnotation },
        //    { EnumToken, TagContextTypes.ParameterAnnotation },
        //    { ForceEncryptionToken, TagContextTypes.ParameterAnnotation },
        //    { HtmlToken, TagContextTypes.ParameterAnnotation },
        //    { InputToken, TagContextTypes.ParameterAnnotation },
        //    { InOutToken, TagContextTypes.ParameterAnnotation },
        //    { MaxlengthToken, TagContextTypes.ParameterAnnotation },
        //    { MinLengthToken, TagContextTypes.ParameterAnnotation },
        //    { OutputToken, TagContextTypes.ParameterAnnotation },
        //    { PasswordToken, TagContextTypes.ParameterAnnotation },
        //    { PhoneToken, TagContextTypes.ParameterAnnotation },
        //    { PostalCodeToken, TagContextTypes.ParameterAnnotation },
        //    { RangeToken, TagContextTypes.ParameterAnnotation },
        //    { RegExPatternToken, TagContextTypes.ParameterAnnotation },
        //    { RegExResourceToken, TagContextTypes.ParameterAnnotation },
        //    { RequiredToken, TagContextTypes.ParameterAnnotation },
        //    { StringLengthToken, TagContextTypes.ParameterAnnotation },
        //    { UrlToken, TagContextTypes.ParameterAnnotation },
        //    { RoutineToken, TagContextTypes.Routine },
        //    { ReturnToken, TagContextTypes.Return },
        //    { QueryStartToken, TagContextTypes.Query },
        //    { QueryEndToken, TagContextTypes.Query },
        //    { ParameterSection, TagContextTypes.ParameterSection }
        //};

    }
        public string AnnotationNamespace { get; }
        public string PrimaryTagPrefix { get; }
        public string SupplementalTagPrefix { get; }
        public string ExplicitTag { get; }
        public string CreditCardTag { get; }
        public string CommentTag { get; }
        public string CurrencyTag {get; }
        public string DefaultTag { get; }
        public string DisplayTag {get; }
        public string EmailTag { get; }
        public string EnumTag { get; }
        public string ForceColumnEncryptionTag { get; }
        public string HtmlTag  { get; }
        public string IgnoreTag { get; }
        public string InputTag {get; }
        public string InOutTag {get; }
        public string MaxlengthTag {get; }
        public string MinLengthTag {get; }
        public string OutputTag {get; }
        public string ParametersTag { get; }
        public string PasswordTag {get; }
        public string PhoneTag {get; }
        public string PostalCodeTag {get; }
        public string RangeTag { get; }
        public string RegExPatternTag { get; }
        public string ReturnTag { get; }
        public string RequiredTag { get; }
        public string RoutineTag { get; }
        public string StringLengthTag {get; }
        public string QueryStartTag {get; }
        public string QueryEndTag {get; }
        public string UrlTag { get; }
        
        public BaseTag TagFromLine(string line)
        {
            string lowered = line.ToLower();
            BaseTag result = null;

            if (lowered == CreditCardTag)
            {
                result = new CreditCard(PrimaryTagPrefix, SupplementalTagPrefix);
            }
            else if (lowered.StartsWith(CommentTag))
            {
                result = new Comment(PrimaryTagPrefix);
                result.SetPrimary(line);
            }
            else if (lowered == CurrencyTag)
            {
                result = new Currency(PrimaryTagPrefix, SupplementalTagPrefix);
            }
            else if (lowered.StartsWith(DefaultTag))
            {
                result = new Default(PrimaryTagPrefix);
                result.SetPrimary(line);
            }
            else if (lowered.StartsWith(DisplayTag))
            {
                result = new Display(PrimaryTagPrefix, SupplementalTagPrefix);
                result.SetPrimary(line);
            }
            else if (lowered == EmailTag)
            {
                result = new Email(PrimaryTagPrefix, SupplementalTagPrefix);
            }
            else if (lowered.StartsWith(EnumTag))
            {
                result = new Enum(PrimaryTagPrefix);
                result.SetPrimary(line);
            }
            else if(lowered.StartsWith(ExplicitTag))
            {
                result = new Explicit(PrimaryTagPrefix);
                result.SetPrimary(line);
            }
            else if (lowered == ForceColumnEncryptionTag)
            {
                result = new ForceColumnEncryption(PrimaryTagPrefix);
            }
            else if (lowered == HtmlTag)
            {
                result = new Html(PrimaryTagPrefix, SupplementalTagPrefix);
            }
            else if (lowered.StartsWith(IgnoreTag))
            {
                result = new Ignore(PrimaryTagPrefix);
            }
            else if(lowered == InOutTag)
            {
                result = new InOut(PrimaryTagPrefix);
            }
            else if (lowered == InputTag)
            {
                result = new Input(PrimaryTagPrefix);
            }
            else if (lowered.StartsWith(MaxlengthTag))
            {
                result = new MaxLength(PrimaryTagPrefix, SupplementalTagPrefix);
                result.SetPrimary(line);
            }
            else if (lowered.StartsWith(MinLengthTag))
            {
                result = new MinLength(PrimaryTagPrefix, SupplementalTagPrefix);
                result.SetPrimary(line);
            }
            else if (lowered == OutputTag)
            {
                result = new Output(PrimaryTagPrefix);
            }
            else if (lowered == ParametersTag)
            {
                result = new Output(PrimaryTagPrefix);
            }
            else if (lowered == PasswordTag)
            {
                result = new Password(PrimaryTagPrefix, SupplementalTagPrefix);
            }
            else if (lowered == PhoneTag)
            {
                result = new Phone(PrimaryTagPrefix, SupplementalTagPrefix);
            }
            else if (lowered == PostalCodeTag)
            {
                result = new PostalCode(PrimaryTagPrefix, SupplementalTagPrefix);
            }
            else if(lowered.StartsWith(QueryStartTag))
            {
                result = new QueryStart(PrimaryTagPrefix);
                result.SetPrimary(line);
            }
            else if (lowered.StartsWith(RangeTag))
            {
                result = new Range(PrimaryTagPrefix, SupplementalTagPrefix);
                result.SetPrimary(line);
            }
            else if (lowered.StartsWith(RegExPatternTag))
            {
                result = new RegExPattern(PrimaryTagPrefix, SupplementalTagPrefix);
                result.SetPrimary(line);
            }
            else if (lowered.StartsWith(StringLengthTag))
            {
                result = new StringLength(PrimaryTagPrefix, SupplementalTagPrefix);
                result.SetPrimary(line);
            }
            else if (lowered == UrlTag)
            {
                result = new Url(PrimaryTagPrefix, SupplementalTagPrefix);
            }
            else if (lowered == RequiredTag)
            {
                result = new Required(PrimaryTagPrefix, SupplementalTagPrefix);
            }
            else if (lowered == RoutineTag)
            {
                result = new SQLPlusRoutine(PrimaryTagPrefix, SupplementalTagPrefix);
            }
            else if(lowered.StartsWith(ReturnTag,StringComparison.OrdinalIgnoreCase))
            {
                result = new Return(PrimaryTagPrefix);
                result.SetPrimary(line);
            }
            if(result == null)
            {
                throw new Exception($"Invalid tag found {line}");
            }

            return result;
        }
    }
}
