import { ApiService, ApiServiceDefinition } from "../services/ApiService";
import {
  CreateGameRequest,
  CreateGameResponse,
  createGameResponseSchema,
} from "./CreateGame/createGame.schema";
import {
  CreateEntryRequest,
  CreateEntryRequestBody,
  CreateEntryResponse,
  GetGameRequest,
  GetGameResponse,
  createEntryResponseSchema,
  getGameResponseSchema,
} from "./GameDetail/game.schema";
import {
  SearchGamesRequest,
  SearchGamesResponse,
  searchGamesResponseSchema,
} from "./games.schema";

interface GameService {
  createGame(request: CreateGameRequest): Promise<CreateGameResponse>;
  searchGames(request: SearchGamesRequest): Promise<SearchGamesResponse>;
  getGame(request: GetGameRequest): Promise<GetGameResponse>;
  createEntry(request: CreateEntryRequest): Promise<CreateEntryResponse>;
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

  const getGame: GameService["getGame"] = async (request) => {
    const result = await api.get(`/game/${request.route.id}`);
    if (!result.success) throw result.error;

    const valid = getGameResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    console.log(valid.error);
    throw new Error(valid.error.errors.map((e) => e.message).join("."));
  };

  const createEntry: GameService["createEntry"] = async (request) => {
    const result = await api.post<CreateEntryRequestBody, CreateEntryResponse>(
      "/entry",
      request.body,
      {
        withCredentials: true,
      }
    );
    if (!result.success) throw result.error;

    const valid = createEntryResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.errors.map((e) => e.message);
  };

  return { createGame, searchGames, getGame, createEntry };
}

export default createGameService({
  api: new ApiService({ baseUrl: "/lotteryapi" }),
});
