FROM quay.io/keycloak/keycloak:latest


# ===== Realm import =====
COPY realm-export.json /opt/keycloak/data/import/

# ===== Build optimized Keycloak =====
RUN /opt/keycloak/bin/kc.sh build --db=postgres

ENTRYPOINT ["/opt/keycloak/bin/kc.sh", "start", "--optimized", "--import-realm"]