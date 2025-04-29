# Dockerfile.act-dind
FROM docker:dind

# Install bash, git & Act
RUN apk add --no-cache bash git curl \
 && curl -sL https://raw.githubusercontent.com/nektos/act/master/install.sh | bash

# Disable Docker’s TLS bootstrap (not needed inside DinD)
ENV DOCKER_TLS_CERTDIR=""

# Entrypoint: start dockerd, then run Act
ENTRYPOINT ["dockerd-entrypoint.sh"]
CMD ["act", "-j", "build"]
