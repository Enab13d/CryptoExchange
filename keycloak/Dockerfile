# Use official Keycloak image (do NOT use latest in prod)
FROM quay.io/keycloak/keycloak:25.0.2

# Copy your realm export into the import folder
COPY realm-export.json /opt/keycloak/data/import/

# Use entrypoint that imports the realm
ENTRYPOINT ["/opt/keycloak/bin/kc.sh", "start", "--import-realm", "--optimized"]
