FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ChallengeAPI.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish ChallengeAPI.csproj -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
RUN addgroup --system appgroup && \
    adduser --system --ingroup appgroup --no-create-home appuser
COPY --from=build /out .
RUN chown -R appuser:appgroup /app
USER appuser
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "ChallengeAPI.dll"]