import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import gameService from "./gameService";
import { SearchGamesResponseItem } from "./CreateGame/createGame.schema";
import {
  Center,
  Container,
  Flex,
  Pagination,
  Paper,
  Select,
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
      <Paper shadow="xl" p="xl">
        <Flex wrap={"wrap"} gap={"lg"} justify={"center"} flex={"1 1 0"}>
          {(
            query.data?.items ??
            Array(limit).fill({
              name: "dummy game info",
              startTime: new Date(),
              drawTime: new Date(),
            } as SearchGamesResponseItem)
          ).map((game, i) => (
            <GameCard key={i} {...game} loading={query.isLoading} />
          ))}
        </Flex>
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
