FROM mcr.microsoft.com/mssql/server:2022-latest

COPY ./wait-and-run.sh /wait-and-run.sh
COPY ./init.sql /init.sql

CMD /wait-and-run.sh