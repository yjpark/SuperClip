FROM mcr.microsoft.com/dotnet/core/runtime:2.2

RUN apt-get update

COPY src/SuperClip.Tools/bin/Debug/netcoreapp2.1/publish /SuperClip.Tools
RUN ls -l /SuperClip.Tools

COPY src/SuperClip.Web/bin/Debug/netcoreapp2.1/publish /SuperClip.Web
COPY src/SuperClip.Web/wwwroot /SuperClip.Web/wwwroot
RUN ls -l /SuperClip.Web

WORKDIR /SuperClip.Web/
EXPOSE 5700

ENV DB_URL=change_this_value
ENTRYPOINT ["dotnet", "SuperClip.Web.dll"]
