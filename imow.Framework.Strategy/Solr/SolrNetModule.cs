using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using HttpWebAdapters;
using SolrNet;
using SolrNet.Impl;
using SolrNet.Impl.DocumentPropertyVisitors;
using SolrNet.Impl.FacetQuerySerializers;
using SolrNet.Impl.FieldParsers;
using SolrNet.Impl.FieldSerializers;
using SolrNet.Impl.QuerySerializers;
using SolrNet.Impl.ResponseParsers;
using SolrNet.Mapping;
using SolrNet.Mapping.Validation;
using SolrNet.Mapping.Validation.Rules;
using SolrNet.Schema;
using SolrNet.Utils;

namespace imow.Framework.Strategy.Solr
{
    /// <summary>
    /// Configures SolrNet in an Autofac container
    /// </summary>
    public class SolrNetModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            if (!string.IsNullOrEmpty(ServerUrl))
                RegisterSingleCore(builder);
         
        }

        /// <summary>
        ///   Register a single-core server
        /// </summary>
        /// <param name = "serverUrl"></param>
        public SolrNetModule(string serverUrl)
        {
            ServerUrl = serverUrl;
        }

        private readonly string ServerUrl;

        /// <summary>
        /// Optional override for document mapper
        /// </summary>
        public IReadOnlyMappingManager Mapper { get; set; }

        /// <summary>
        /// Optional override to provide a different <see cref="IHttpWebRequestFactory"/>.
        /// </summary>
        public IHttpWebRequestFactory HttpWebRequestFactory { get; set; }

        private void RegisterCommonComponents(ContainerBuilder builder)
        {
            var mapper = Mapper ?? new MemoizingMappingManager(new AttributesMappingManager());
            builder.RegisterInstance(mapper).As<IReadOnlyMappingManager>();
           builder.RegisterType<HttpRuntimeCache>().As<ISolrCache>();
            builder.RegisterType<DefaultDocumentVisitor>().As<ISolrDocumentPropertyVisitor>();
            builder.RegisterType<DefaultFieldParser>().As<ISolrFieldParser>();
            builder.RegisterGeneric(typeof(SolrDocumentActivator<>)).As(typeof(ISolrDocumentActivator<>));
            builder.RegisterGeneric(typeof(SolrDocumentResponseParser<>)).As(typeof(ISolrDocumentResponseParser<>));
            builder.RegisterType<DefaultFieldSerializer>().As<ISolrFieldSerializer>();
            builder.RegisterType<DefaultQuerySerializer>().As<ISolrQuerySerializer>();
            builder.RegisterType<DefaultFacetQuerySerializer>().As<ISolrFacetQuerySerializer>();
            builder.RegisterGeneric(typeof(DefaultResponseParser<>)).As(typeof(ISolrAbstractResponseParser<>));

            builder.RegisterType<HeaderResponseParser<string>>().As<ISolrHeaderResponseParser>();
            builder.RegisterType<ExtractResponseParser>().As<ISolrExtractResponseParser>();
            foreach (var p in new[] {
                typeof (MappedPropertiesIsInSolrSchemaRule),
                typeof (RequiredFieldsAreMappedRule),
                typeof (UniqueKeyMatchesMappingRule),
                typeof(MultivaluedMappedToCollectionRule),
            })

                builder.RegisterType(p).As<IValidationRule>();
            builder.RegisterType<SolrSchemaParser>().As<ISolrSchemaParser>();
            builder.RegisterGeneric(typeof(SolrMoreLikeThisHandlerQueryResultsParser<>)).As(typeof(ISolrMoreLikeThisHandlerQueryResultsParser<>));
            builder.RegisterGeneric(typeof(SolrQueryExecuter<>)).As(typeof(ISolrQueryExecuter<>));
            builder.RegisterGeneric(typeof(SolrDocumentSerializer<>)).As(typeof(ISolrDocumentSerializer<>));
            builder.RegisterType<SolrDIHStatusParser>().As<ISolrDIHStatusParser>();
            builder.RegisterType<MappingValidator>().As<IMappingValidator>();
            builder.RegisterType<SolrStatusResponseParser>().As<ISolrStatusResponseParser>();
            builder.RegisterType<SolrCoreAdmin>().As<ISolrCoreAdmin>();
            builder.RegisterType<SolrDictionarySerializer>().As<ISolrDocumentSerializer<Dictionary<string, object>>>();
            builder.RegisterType<SolrDictionaryDocumentResponseParser>().As<ISolrDocumentResponseParser<Dictionary<string, object>>>();
        }

        private void RegisterSingleCore(ContainerBuilder builder)
        {
            RegisterCommonComponents(builder);

            SolrConnection solrConnection = new SolrConnection(ServerUrl);
            if (HttpWebRequestFactory != null)
            {
                solrConnection.HttpWebRequestFactory = HttpWebRequestFactory;
            }
            builder.RegisterInstance(solrConnection).As<ISolrConnection>();

            builder.RegisterGeneric(typeof(SolrBasicServer<>))
                .As(typeof(ISolrBasicOperations<>), typeof(ISolrBasicReadOnlyOperations<>))
                .SingleInstance();
            builder.RegisterGeneric(typeof(SolrServer<>))
                .As(typeof(ISolrOperations<>), typeof(ISolrReadOnlyOperations<>))
                .SingleInstance();
        }


      

      

    
    }
}