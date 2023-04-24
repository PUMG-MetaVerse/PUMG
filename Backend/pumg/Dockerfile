FROM openjdk:11
ARG JAR_FILE_PATH=build/libs/pumg-0.0.1-SNAPSHOT.jar
COPY ${JAR_FILE_PATH} app.jar
EXPOSE 6999
ENTRYPOINT ["java", "-jar", "/app.jar"]