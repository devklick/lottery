import { ApiService, ApiServiceDefinition } from "../services/ApiService";
import {
  CreateGameRequest,
  CreateGameResponse,
} from "./CreateGame/createGame.schema";

interface GameService {
  createGame(request: CreateGameRequest): Promise<CreateGameResponse>;
}

export function createGameService({
  api,
}: {
  api: ApiServiceDefinition;
}): GameService {
  const createGame: GameService["createGame"] = async (
    request
  ): Promise<CreateGameResponse> => {
    const result = await api.post<CreateGameRequest, CreateGameResponse>(
      "/game",
      request
    );
    if (result.success) return result.data;
    throw result.error;
  };

  return { createGame };
}

export default createGameService({
  api: new ApiService({ baseUrl: "/lotteryapi" }),
});
