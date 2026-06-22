import { Grid } from "@mantine/core";
import { useQuery } from "@tanstack/react-query";
import { useMemo } from "react";

import GameCard from "./GameCard";
import GameFilters from "./GameFilters";
import { SearchGamesRequest, SearchGamesResponseItem } from "./games.schema";
import gameService from "./gameService";
import Page from "../components/Page/Page";
import PageSection from "../components/PageSection/PageSection";
import PaginationBar from "../components/PaginationBar/PaginationBar";
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
  } as unknown as SearchGamesRequest;
  // TODO: Implement this properly at some point...
}

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface GamesProps {}

// eslint-disable-next-line no-empty-pattern
function Games({}: GamesProps) {
  const [searchParams, setSearchParams] = useMergedSearchParams(defaultFilters);
  const filters = useMemo(
    () => searchParamsToFilters(searchParams),
    [searchParams],
  );

  const updateSearchParams = (updates: Partial<SearchGamesRequest>) =>
    setSearchParams(new QueryParams({ ...filters, ...updates }));

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
    <Page
      title={{ value: "Lottery Games" }}
      children={
        <PageSection width={"100%"}>
          <GameFilters
            initialValues={filters}
            onUpdateClicked={(newFilters) =>
              setSearchParams(new QueryParams({ ...filters, ...newFilters }))
            }
          />
          <Grid
            gap={{ base: 24, md: "xl", xl: 50 }}
            w="100%"
            justify={"center"}
          >
            {(query.data?.items ?? placeholder).map((game, i) => (
              <Grid.Col
                key={`game-${game.id}`}
                style={{ alignSelf: "stretch" }}
                span={{ xs: 12, sm: 6, md: 4, lg: 4, xl: 4 }}
              >
                <GameCard
                  key={i}
                  {...game}
                  numbersRequired={
                    game.prizes.find((p) => p.position === 1)
                      ?.numberMatchCount ?? 5
                  }
                  selectionNumbers={game.selections.map(
                    (s) => s.selectionNumber,
                  )}
                  gameStatus={game.gameStatus}
                  loading={query.isLoading}
                />
              </Grid.Col>
            ))}
          </Grid>
        </PageSection>
      }
      footer={
        <PaginationBar
          limit={filters.limit}
          onLimitChanged={(limit) => updateSearchParams({ limit })}
          onPageChanged={(page) => updateSearchParams({ page })}
          page={filters.page}
          totalItems={query.data?.total}
          limits={[12, 24, 48]}
        />
      }
    />
  );
}

export default Games;
