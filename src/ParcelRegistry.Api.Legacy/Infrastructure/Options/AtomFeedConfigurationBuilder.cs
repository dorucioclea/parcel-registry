namespace ParcelRegistry.Api.Legacy.Infrastructure.Options
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mime;
    using System.Reflection;
    using Be.Vlaanderen.Basisregisters.Api.Syndication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.SyndicationFeed;
    using Microsoft.SyndicationFeed.Atom;

    public class AtomFeedConfigurationBuilder
    {
        public static AtomFeedConfiguration CreateFrom(IConfigurationSection configuration, DateTimeOffset lastUpdated)
        {
            return new AtomFeedConfiguration(
                configuration["Id"],
                configuration["Title"],
                configuration["Subtitle"],
                configuration["GeneratorTitle"],
                configuration["GeneratorUri"],
                Assembly.GetEntryAssembly().GetName().Version.ToString(),
                configuration["Rights"],
                lastUpdated,
                new SyndicationPerson(configuration["AuthorName"], configuration["AuthorEmail"], AtomContributorTypes.Author),
                new SyndicationLink(new Uri(configuration["Self"]), AtomLinkTypes.Self),
                new List<SyndicationLink>
                {
                    new SyndicationLink(new Uri(configuration["AlternateAtom"]), AtomLinkTypes.Alternate){ MediaType = "application/atom+xml"},
                    new SyndicationLink(new Uri(configuration["AlternateXml"]), AtomLinkTypes.Alternate){ MediaType = MediaTypeNames.Application.Xml}
                },
                configuration
                    .GetSection("Related")
                    .GetChildren()
                    .Select(c =>
                        new SyndicationLink(new Uri(c.Value), AtomLinkTypes.Related))
                    .ToList()
            );
        }
    }
}
