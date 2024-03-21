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

interface GamesProps {}

function Games({}: GamesProps) {
  const [page, setPage] = useState<number>(1);
  const [limit, setLimit] = useState<number>(10);
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
    <Container>
      <Title>Lottery Games</Title>

      <Paper
        shadow="xl"
        p={{ xl: "xl", lg: "xl", mx: "xl", sm: "xl", xs: "xs" }}
        m={{ xl: "xl", lg: "xl", mx: "xl", sm: "xl", xs: "xs" }}
      >
        <GameFilters
          initialValues={filters}
          onUpdateClicked={(filters) => setFilters(filters)}
        />
        <Grid
          gutter={{ base: 5, xs: "md", md: "xl", xl: 50 }}
          justify={"center"}
          grow
        >
          {(
            query.data?.items ??
            Array(limit).fill({
              name: "dummy game info",
              startTime: new Date(),
              drawTime: new Date(),
            } as SearchGamesResponseItem)
          ).map((game, i) => (
            <Grid.Col span={{ xs: 12, sm: 6, md: 4, lg: 4, xl: 4 }}>
              <GameCard key={i} {...game} loading={query.isLoading} />
            </Grid.Col>
          ))}
        </Grid>
      </Paper>

      <Center w={"100%"} mx={5} mt={50}>
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
            data={["10", "20", "50"]}
            onChange={(value) => setLimit(Number(value))}
            allowDeselect={false}
          />
        </Flex>
      </Center>
    </Container>
  );
}

export default Games;
