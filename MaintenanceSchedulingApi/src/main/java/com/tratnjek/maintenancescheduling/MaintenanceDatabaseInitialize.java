package com.tratnjek.maintenancescheduling;

import io.quarkus.runtime.StartupEvent;
import io.quarkus.runtime.configuration.ProfileManager;
import io.vertx.mutiny.pgclient.PgPool;
import jakarta.enterprise.context.ApplicationScoped;
import jakarta.enterprise.event.Observes;
import jakarta.inject.Inject;

@ApplicationScoped
public class MaintenanceDatabaseInitialize {

    @Inject
    PgPool client;

void onStart(@Observes StartupEvent ev) {
    if(!ProfileManager.getLaunchMode().isDevOrTest()){
        client.query("CREATE TABLE IF NOT EXISTS maintenance (id SERIAL PRIMARY KEY, name TEXT, description TEXT, location TEXT, duration TEXT, status TEXT, type TEXT, priority TEXT, notes TEXT)")
                .execute()
                .await().indefinitely();
        }
    }
}
