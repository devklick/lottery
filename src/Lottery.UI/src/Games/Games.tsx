import { useQuery } from "@tanstack/react-query";
import { useEffect, useRef, useState } from "react";
import gameService from "./gameService";
import {
  Center,
  Container,
  Flex,
  Grid,
  Pagination,
  Paper,
  Select,
  Title,
} from "@mantine/core";
import GameCard from "./GameCard";
import GameFilters from "./GameFilters";
import { SearchGamesRequest, SearchGamesResponseItem } from "./games.schema";
import useMergedSearchParams from "../hooks/url/useMergedSearchParams";
import QueryParams from "../utils/QueryParams";

const placeholder: Array<SearchGamesResponseItem> = Array.from<
  SearchGamesResponseItem,
  SearchGamesResponseItem
>({ length: 12 }, (_, i) => ({
  name: "Skeleton",
  startTime: new Date(),
  closeTime: new Date(),
  gameStatus: "closed",
  drawTime: new Date(),
  id: i.toString(),
  prizes: [],
  selections: [],
  selectionsRequiredForEntry: 5,
}));

const defaultFilters: SearchGamesRequest = {
  gameStatus: ["open", "future"],
  sortBy: "drawTime",
  sortDirection: "desc",
  name: "",
  page: 1,
  limit: 12,
};

function searchParamsToFilters(params: URLSearchParams): SearchGamesRequest {
  return {
    gameStatus: params.get("gameStatus")
      ? Array.isArray(params.get("gameStatus")) &&
        params.get("gameStatus")?.length === 1
        ? [params.get("gameStatus")]
        : params.getAll("gameStatus")
      : defaultFilters.gameStatus,
    limit: params.get("limit") ?? defaultFilters.limit,
    page: params.get("page") ?? defaultFilters.page,
    sortBy: params.get("sortBy") ?? defaultFilters.sortBy,
    sortDirection: params.get("sortDirection") ?? defaultFilters.sortDirection,
    name: params.get("name") ?? defaultFilters.name,
  } as any as SearchGamesRequest;
  // TODO: Implement this properly at some point...
}

interface GamesProps {}

function Games({}: GamesProps) {
  const [searchParams, setSearchParams] = useMergedSearchParams(defaultFilters);
  const [filters, setFilters] = useState<SearchGamesRequest>(
    searchParamsToFilters(searchParams)
  );

  useEffect(() => {
    if (searchParams.size) {
      setFilters(searchParamsToFilters(searchParams));
    }
  }, [searchParams]);

  const query = useQuery({
    queryKey: [
      "game",
      "search",
      filters.page,
      filters.limit,
      filters.gameStatus,
      filters.name,
      filters.sortBy,
      filters.sortDirection,
    ],
    queryFn: () =>
      gameService.searchGames({
        limit: filters.limit,
        page: filters.page,
        gameStatus: filters.gameStatus,
        sortBy: filters.sortBy,
        sortDirection: filters.sortDirection,
        name: filters.name,
      }),
  });

  return (
    <Container p={0}>
      <Title>Lottery Games</Title>

      <Paper shadow="xl" p={24} radius={10}>
        <GameFilters
          initialValues={filters}
          onUpdateClicked={(newFilters) =>
            setSearchParams(new QueryParams({ ...filters, ...newFilters }))
          }
        />
        <Grid gutter={{ base: 24, md: "xl", xl: 50 }} justify={"center"}>
          {(query.data?.items ?? placeholder).map((game, i) => (
            <Grid.Col
              key={`game-${game.id}`}
              style={{ alignSelf: "stretch" }}
              span={{ xs: 12, sm: 6, md: 4, lg: 4, xl: 4 }}
            >
              <GameCard
                key={i}
                {...game}
                numbersRequired={game.selectionsRequiredForEntry}
                selectionNumbers={game.selections.map((s) => s.selectionNumber)}
                gameStatus={game.gameStatus}
                loading={query.isLoading}
              />
            </Grid.Col>
          ))}
        </Grid>
      </Paper>

      <Center w={"100%"} mt={50}>
        <Flex gap={"lg"} align={"center"}>
          <Pagination
            total={Math.max(
              Math.ceil((query.data?.total ?? 0) / filters.limit),
              1
            )}
            value={filters.page}
            onChange={(value) =>
              setSearchParams(
                new QueryParams({ ...filters, page: Number(value) })
              )
            }
          />
          <Select
            w={80}
            value={filters.limit.toString()}
            defaultValue={filters.limit.toString()}
            data={["12", "24", "48"]}
            onChange={(value) =>
              setSearchParams(
                new QueryParams({ ...filters, limit: Number(value) })
              )
            }
            allowDeselect={false}
          />
        </Flex>
      </Center>
    </Container>
  );
}

export default Games;
