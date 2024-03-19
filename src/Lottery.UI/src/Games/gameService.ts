import { ApiService, ApiServiceDefinition } from "../services/ApiService";
import {
  CreateGameRequest,
  CreateGameResponse,
  SearchGamesRequest,
  SearchGamesResponse,
  createGameResponseSchema,
  searchGamesResponseSchema,
} from "./CreateGame/createGame.schema";

interface GameService {
  createGame(request: CreateGameRequest): Promise<CreateGameResponse>;
  searchGames(request: SearchGamesRequest): Promise<SearchGamesResponse>;
}

export function createGameService({
  api,
}: {
  api: ApiServiceDefinition;
}): GameService {
  const createGame: GameService["createGame"] = async (request) => {
    const result = await api.post<CreateGameRequest, CreateGameResponse>(
      "/game",
      request,
      { withCredentials: true }
    );
    if (!result.success) throw result.error;

    const valid = createGameResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw new Error(valid.error.errors.map((e) => e.message).join(". "));
  };

  const searchGames: GameService["searchGames"] = async (request) => {
    const result = await api.get<SearchGamesRequest, SearchGamesResponse>(
      "/game/search",
      request
    );
    if (!result.success) throw result.error;

    const valid = searchGamesResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.errors.map((e) => e.message);
  };

  return { createGame, searchGames };
}

export default createGameService({
  api: new ApiService({ baseUrl: "/lotteryapi" }),
});
