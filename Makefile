ifeq ($(VERSION),)
	FILENAME = nanny_1.0.0.0
else
	FILENAME = nanny_$(VERSION)
endif

release:
	make release-deb
	make release-brew

release-deb:
	sed -e "s|VERSION|$$VERSION|" ./Packaging/debian/DEBIAN/control.txt > ./Packaging/debian/DEBIAN/control
	dotnet publish Nanny.Console/Nanny.Console.csproj -c Release --self-contained -r ubuntu.20.04-x64 -o Packaging/debian/opt/kolenkainc/nanny
	cp -R Packaging/debian $(FILENAME)
	dpkg-deb --build $(FILENAME)
	rm -rf $(FILENAME)

release-brew:
	dotnet publish Nanny.Console/Nanny.Console.csproj -c Release --self-contained -r osx.10.12-x64 -o bin
	tar -cvzf $(FILENAME).tar.gz bin

publish-brew:
	wget https://github.com/kolenkainc/Nanny/releases/download/$(VERSION)/$(FILENAME).tar.gz
	# change version of formula
	cp -R Packaging/src/brew Packaging/dist
	sed -i.bak "s/VERSION/$$VERSION/g" Packaging/dist/brew/Formula/nanny.rb && rm Packaging/dist/brew/Formula/nanny.rb.bak
	# calculate sha and change it in formula
	openssl dgst -sha256 $(FILENAME).tar.gz > Packaging/dist/brew/Formula/sha256.txt
	sed -i.bak "s/SHA256($(FILENAME).tar.gz)= //g" Packaging/dist/brew/Formula/sha256.txt && rm Packaging/dist/brew/Formula/sha256.txt.bak
	SHA=$$(cat Packaging/dist/brew/Formula/sha256.txt) && sed -i.bak "s/SHA/$$SHA/g" Packaging/dist/brew/Formula/nanny.rb && rm Packaging/dist/brew/Formula/nanny.rb.bak && rm Packaging/dist/brew/Formula/sha256.txt
