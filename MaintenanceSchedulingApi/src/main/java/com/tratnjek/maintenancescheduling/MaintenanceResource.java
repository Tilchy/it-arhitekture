package com.tratnjek.maintenancescheduling;

import io.smallrye.mutiny.Uni;
import io.smallrye.reactive.messaging.annotations.Broadcast;
import jakarta.enterprise.context.ApplicationScoped;
import jakarta.inject.Inject;
import jakarta.ws.rs.*;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
import org.eclipse.microprofile.reactive.messaging.Channel;
import org.eclipse.microprofile.reactive.messaging.Emitter;
import org.eclipse.microprofile.reactive.messaging.Incoming;

import java.util.List;
import java.util.logging.Logger;

@ApplicationScoped
@Path("/maintenance")
@Produces(MediaType.APPLICATION_JSON)
public class MaintenanceResource {

    private final Logger logger = Logger.getLogger(MaintenanceResource.class.getName());

    @Inject
    @Broadcast
    @Channel("maintenance")
    Emitter<MaintenanceMessage> maintenanceEmitter;

    @Incoming("maintenance")
    public void testMessages(MaintenanceMessage message){
        logger.info("Received test message: " + message.name + " " + message.status);
    }

    @GET
    public Uni<List<Maintenance>> list() {
        logger.info("Listing all maintenance tasks");
        return Maintenance.listAll();
    }

    @GET
    @Path("/{id}")
    public Uni<Response> get(@PathParam("id") Long id) {
        logger.info("Getting a maintenance task");

        return Maintenance.findById(id)
                .onItem()
                .transform(maintenance -> maintenance != null ? Response.ok(maintenance).build() :
                        Response.status(Response.Status.NOT_FOUND).build());
    }

    @POST
    public Uni<Response> add(Maintenance maintenance) {
        logger.info("Adding a maintenance task");

        return maintenance.persist()
                .onItem().transform(m -> {
                    logger.info("Maintenance task persisted");
                    if (m.isPersistent()) {
                        logger.info("Sending a maintenance message");
                        MaintenanceMessage maintenanceMessage = new MaintenanceMessage();
                        maintenanceMessage.name = maintenance.name;
                        maintenanceMessage.status = maintenance.status;

                        maintenanceEmitter.send(maintenanceMessage);
                        logger.info("Maintenance message sent");

                        return Response.ok(m).build();
                    } else {
                        return Response.status(Response.Status.BAD_REQUEST).build();
                    }
                });
    }

    @PUT
    @Path("/{id}")
    public Uni<Response> update(@PathParam("id") Long id, Maintenance maintenance) {
        logger.info("Updating a maintenance task");
        return Maintenance.<Maintenance>findById(id)
                .onItem().ifNotNull().transformToUni(m -> {
                    m.name = maintenance.name;
                    m.description = maintenance.description;
                    m.location = maintenance.location;
                    //m.scheduled = maintenance.scheduled;
                    m.duration = maintenance.duration;
                    m.status = maintenance.status;
                    m.type = maintenance.type;
                    m.priority = maintenance.priority;
                    return m.persist();
                })
                .map(m -> Response.ok(m).build())
                .replaceIfNullWith(Response.status(Response.Status.NOT_FOUND).build());
    }

    @DELETE
    @Path("/{id}")
    public Uni<Response> delete(@PathParam("id") Long id) {
        logger.info("Deleting a maintenance task");
        return Maintenance.<Maintenance>findById(id)
                .onItem().ifNotNull().transformToUni(m -> m.delete().map(v -> m))
                .map(m -> Response.status(Response.Status.NO_CONTENT).build())
                .replaceIfNullWith(Response.status(Response.Status.NOT_FOUND).build());
    }

}
