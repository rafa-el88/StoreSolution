import { Environment } from "../app/models/environment.model";

export const environment: Environment = {
  production: true,
  baseUrl: null, // Set to null to use the current host (i.e if the client app and server api are hosted together)
};
