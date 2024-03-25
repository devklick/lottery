import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
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
import {
  SearchGamesRequestFilter,
  SearchGamesResponseItem,
} from "./games.schema";

const placeholder: Array<SearchGamesResponseItem> = Array.from<
  SearchGamesResponseItem,
  SearchGamesResponseItem
>({ length: 12 }, (_, i) => ({
  name: "Skeleton",
  startTime: new Date(),
  drawTime: new Date(),
  id: i.toString(),
  prizes: [],
  selections: [],
  selectionsRequiredForEntry: 5,
}));

interface GamesProps {}

function Games({}: GamesProps) {
  const [page, setPage] = useState<number>(1);
  const [limit, setLimit] = useState<number>(12);
  const [filters, setFilters] = useState<SearchGamesRequestFilter>({
    gameStates: ["canEnter", "future"],
    sortBy: "drawTime",
    sortDirection: "desc",
    name: "",
  });

  const query = useQuery({
    queryKey: [
      "game",
      "search",
      page,
      limit,
      filters.gameStates,
      filters.name,
      filters.sortBy,
      filters.sortDirection,
    ],
    queryFn: () =>
      gameService.searchGames({
        limit,
        page,
        gameStates: filters.gameStates,
        sortBy: filters.sortBy,
        sortDirection: filters.sortDirection,
        name: filters.name,
      }),
  });

  return (
    <Container p={0}>
      <Title>Lottery Games</Title>

      <Paper shadow="xl" p={24} radius={10}>
        <GameFilters initialValues={filters} onUpdateClicked={setFilters} />
        <Grid gutter={{ base: 24, md: "xl", xl: 50 }} justify={"center"}>
          {(query.data?.items ?? placeholder).map((game, i) => (
            <Grid.Col
              key={`game-${game.id}`}
              style={{ alignSelf: "stretch" }}
              span={{ xs: 12, sm: 6, md: 4, lg: 4, xl: 4 }}
            >
              <GameCard key={i} {...game} loading={query.isLoading} />
            </Grid.Col>
          ))}
        </Grid>
      </Paper>

      <Center w={"100%"} mt={50}>
        <Flex gap={"lg"} align={"center"}>
          <Pagination
            total={Math.max(Math.ceil((query.data?.total ?? 0) / limit), 1)}
            value={page}
            onChange={setPage}
          />
          <Select
            w={80}
            value={limit.toString()}
            defaultValue={limit.toString()}
            data={["12", "24", "48"]}
            onChange={(value) => setLimit(Number(value))}
            allowDeselect={false}
          />
        </Flex>
      </Center>
    </Container>
  );
}

export default Games;
