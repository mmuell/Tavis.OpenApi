﻿using System.Collections.Generic;
using SharpYaml.Serialization;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tavis.OpenApi.Model
{
    
    public class Info
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TermsOfService {
            get { return this.termsOfService; } 
            set
            {
                if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute))
                {
                    throw new DomainParseException("`info.termsOfService` MUST be a URL");
                };
                this.termsOfService = value;
            }
        }
        string termsOfService;
        public Contact Contact { get; set; }
        public License License { get; set; }

        public string Version { get; set; }

        public Dictionary<string, string> Extensions { get; set; }

        public Info()
        {
            Extensions = new Dictionary<string, string>();
        }

        public static Info Load(ParseNode node)
        {
            var mapNode = node.CheckMapNode("Info");
            var info = new Info();

            var required = new List<string>() { "title", "version" };

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(info, OpenApiParser.InfoFixedFields, OpenApiParser.InfoPatternFields);
                required.Remove(propertyNode.Name);
            }
            node.Context.ParseErrors.AddRange(required.Select(r => new OpenApiError("", $"{r} is a required property")));

            return info;
        }

        private static Regex versionRegex = new Regex(@"\d+\.\d+\.\d+");


    }
        
    }
