FROM docker-registry.fe.abacogroup.eu/abaco/bundle-publisher:v1.1.0

USER root
COPY go.sh .
RUN chmod +x *.sh
COPY ./config.tenants ./config.tenants
COPY ./config ./config

USER nobody
CMD [ "./go.sh" ]
