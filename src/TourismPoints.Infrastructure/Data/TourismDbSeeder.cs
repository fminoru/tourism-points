using Microsoft.EntityFrameworkCore;
using TourismPoints.Domain.Entities;
using TourismPoints.Infrastructure.Context;

namespace TourismPoints.Infrastructure.Data;

public static class TourismDbSeeder
{
    /// <summary>
    /// Inserts sample Brazilian tourist points when the database is empty.
    /// </summary>
    public static async Task SeedAsync(TourismDbContext context, CancellationToken cancellationToken = default)
    {
        if (await context.TouristPoints.AnyAsync(cancellationToken))
            return;

        var t0 = DateTime.UtcNow;
        var points = new[]
        {
            new TouristPoint
            {
                Name = "Cristo Redentor",
                Description = "Monumento Art Déco no Corcovado; vista panorâmica da cidade do Rio de Janeiro.",
                Location = "Parque Nacional da Tijuca — Alto da Boa Vista, Corcovado",
                City = "Rio de Janeiro",
                State = "RJ",
                CreatedAt = t0.AddDays(-20)
            },
            new TouristPoint
            {
                Name = "Pão de Açúcar",
                Description = "Teleférico entre morros com vista da baía de Guanabara e da orla carioca.",
                Location = "Urca — Morro da Urca e Morro do Pão de Açúcar",
                City = "Rio de Janeiro",
                State = "RJ",
                CreatedAt = t0.AddDays(-19)
            },
            new TouristPoint
            {
                Name = "Cataratas do Iguaçu",
                Description = "Uma das maiores quedas d'água do mundo, na fronteira Brasil–Argentina.",
                Location = "Parque Nacional do Iguaçu — Rodovia das Cataratas",
                City = "Foz do Iguaçu",
                State = "PR",
                CreatedAt = t0.AddDays(-18)
            },
            new TouristPoint
            {
                Name = "Pelourinho",
                Description = "Centro histórico barroco, música e cultura afro-brasileira em Salvador.",
                Location = "Praça Terreiro de Jesus — Centro Histórico",
                City = "Salvador",
                State = "BA",
                CreatedAt = t0.AddDays(-17)
            },
            new TouristPoint
            {
                Name = "Teatro Amazonas",
                Description = "Ópera do século XIX com cúpula colorida; ícone da Belle Époque na Amazônia.",
                Location = "Largo de São Sebastião — Centro",
                City = "Manaus",
                State = "AM",
                CreatedAt = t0.AddDays(-16)
            },
            new TouristPoint
            {
                Name = "Congresso Nacional",
                Description = "Sede do Legislativo em Brasília; arquitetura de Oscar Niemeyer.",
                Location = "Praça dos Três Poderes — Eixo Monumental",
                City = "Brasília",
                State = "DF",
                CreatedAt = t0.AddDays(-15)
            },
            new TouristPoint
            {
                Name = "Catedral Metropolitana de Brasília",
                Description = "Templo hiperboloide de vidro e concreto, um dos símbolos da capital.",
                Location = "Esplanada dos Ministérios — Eixo Monumental",
                City = "Brasília",
                State = "DF",
                CreatedAt = t0.AddDays(-14)
            },
            new TouristPoint
            {
                Name = "Fernando de Noronha",
                Description = "Arquipélago marinho com praias, mergulho e trilhas; Patrimônio Natural da UNESCO.",
                Location = "Ilha principal — Vila dos Remédios",
                City = "Fernando de Noronha",
                State = "PE",
                CreatedAt = t0.AddDays(-13)
            },
            new TouristPoint
            {
                Name = "Lençóis Maranhenses",
                Description = "Dunas brancas e lagoas sazonais no litoral do Maranhão.",
                Location = "Parque Nacional dos Lençóis Maranhenses — acesso por Barreirinhas",
                City = "Barreirinhas",
                State = "MA",
                CreatedAt = t0.AddDays(-12)
            },
            new TouristPoint
            {
                Name = "Inhotim",
                Description = "Museu a céu aberto com arte contemporânea e jardim botânico em Minas Gerais.",
                Location = "Rua B — Brumadinho",
                City = "Brumadinho",
                State = "MG",
                CreatedAt = t0.AddDays(-11)
            },
            new TouristPoint
            {
                Name = "Ouro Preto",
                Description = "Cidade colonial barroca, igrejas e museus; Patrimônio Mundial da UNESCO.",
                Location = "Centro histórico — Praça Tiradentes",
                City = "Ouro Preto",
                State = "MG",
                CreatedAt = t0.AddDays(-10)
            },
            new TouristPoint
            {
                Name = "Chapada Diamantina",
                Description = "Cânions, grutas e cachoeiras no sertão baiano; trilhas e ecoturismo.",
                Location = "Parque Nacional da Chapada Diamantina — Lençóis",
                City = "Lençóis",
                State = "BA",
                CreatedAt = t0.AddDays(-9)
            },
            new TouristPoint
            {
                Name = "Bonito",
                Description = "Rios cristalinos, flutuação e grutas; referência em turismo sustentável.",
                Location = "Centro — Município de Bonito",
                City = "Bonito",
                State = "MS",
                CreatedAt = t0.AddDays(-8)
            },
            new TouristPoint
            {
                Name = "Jalapão",
                Description = "Dunas douradas, fervedouros e cânions no cerrado tocantinense.",
                Location = "Região do Jalapão — Mateiros",
                City = "Mateiros",
                State = "TO",
                CreatedAt = t0.AddDays(-7)
            },
            new TouristPoint
            {
                Name = "Porto de Galinhas",
                Description = "Piscinas naturais na maré baixa e praias de coral no litoral sul pernambucano.",
                Location = "Praia de Porto de Galinhas — Ipojuca",
                City = "Ipojuca",
                State = "PE",
                CreatedAt = t0.AddDays(-6)
            },
            new TouristPoint
            {
                Name = "Estádio do Maracanã",
                Description = "Um dos estádios mais famosos do mundo; templo do futebol brasileiro.",
                Location = "Rua Professor Eurico Rocha — Maracanã",
                City = "Rio de Janeiro",
                State = "RJ",
                CreatedAt = t0.AddDays(-5)
            },
            new TouristPoint
            {
                Name = "Escadaria Selarón",
                Description = "Degraus de azulejos coloridos ligando Santa Teresa à Lapa no Rio.",
                Location = "Rua Manuel Carneiro — Santa Teresa / Lapa",
                City = "Rio de Janeiro",
                State = "RJ",
                CreatedAt = t0.AddDays(-4)
            },
            new TouristPoint
            {
                Name = "Elevador Lacerda",
                Description = "Liga a Cidade Alta ao Comércio; vista da Baía de Todos-os-Santos.",
                Location = "Praça Cayru — Cidade Baixa / Pelourinho",
                City = "Salvador",
                State = "BA",
                CreatedAt = t0.AddDays(-3)
            },
            new TouristPoint
            {
                Name = "Mercado Municipal de São Paulo",
                Description = "Mercadão histórico com arquitetura neoclássica e gastronomia paulistana.",
                Location = "Rua da Cantareira — Centro",
                City = "São Paulo",
                State = "SP",
                CreatedAt = t0.AddDays(-2)
            },
            new TouristPoint
            {
                Name = "Avenida Paulista",
                Description = "Eixo cultural e financeiro com museus, parque e eventos ao ar livre.",
                Location = "Avenida Paulista — Bela Vista / Jardins",
                City = "São Paulo",
                State = "SP",
                CreatedAt = t0.AddDays(-1)
            }
        };

        await context.TouristPoints.AddRangeAsync(points, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
