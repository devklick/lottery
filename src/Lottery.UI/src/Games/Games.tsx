import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import gameService from "./gameService";
import { SearchGamesResponseItem } from "./CreateGame/createGame.schema";
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

interface GamesProps {}

function Games({}: GamesProps) {
  const [page, setPage] = useState<number>(1);
  const [limit, setLimit] = useState<number>(10);

  const query = useQuery({
    queryKey: ["game", "search", page, limit],
    queryFn: () => gameService.searchGames({ limit, page }),
  });

  return (
    <Container pos="relative">
      <Title>Lottery Games</Title>
      <Paper shadow="xl" p="xl">
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
            total={Math.ceil((query.data?.total ?? 0) / limit)}
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
