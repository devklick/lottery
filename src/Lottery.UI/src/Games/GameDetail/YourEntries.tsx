import {
  Center,
  Collapse,
  Flex,
  Group,
  Pagination,
  Select,
  Stack,
  Title,
} from "@mantine/core";
import { useUserStore } from "../../stores/user.store";
import { useDisclosure, usePagination } from "@mantine/hooks";
import { IconChevronDown, IconChevronUp } from "@tabler/icons-react";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import gameService from "../gameService";

interface YourEntriesProps {}

function YourEntries({}: YourEntriesProps) {
  const user = useUserStore();
  const [opened, { toggle }] = useDisclosure(false);
  const [page, setPage] = useState(1);
  const [limit, setLimit] = useState(1);

  const query = useQuery({
    queryKey: ["entries", page, limit],
    queryFn: () => gameService.getEntries({ query: { limit, page } }),
    enabled: user.authenticated,
  });

  return (
    <Stack align="center" justify="center" mt={50}>
      <Group style={{ alignSelf: "start" }} onClick={toggle}>
        <Title size={"h2"}>{`Your entries`}</Title>
        {opened ? <IconChevronUp /> : <IconChevronDown />}
      </Group>
      <Collapse in={opened}>
        {user.authenticated ? "Stuff" : "Sign in to view your entries"}
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
              data={["1", "10", "20"]}
              onChange={(value) => setLimit(Number(value))}
              allowDeselect={false}
            />
          </Flex>
        </Center>
      </Collapse>
    </Stack>
  );
}

export default YourEntries;
